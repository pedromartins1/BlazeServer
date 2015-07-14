using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazeServer
{
    public class TdfDecoder
    {
        private byte[] _data;
        private MemoryStream _stream;
        private Dictionary<string, Tdf> _result;

        public TdfDecoder(byte[] data)
        {
            _data = data;
            _stream = new MemoryStream(_data);
            _result = new Dictionary<string, Tdf>();
        }

        /// <summary>
        /// Decodes a tag to a label.
        /// </summary>
        private string ReadLabel()
        {
            // by Pedro Martins
            string label = "";
            uint tag = 0;

            // get tag bytes
            byte[] tagBytes = new byte[3];
            _stream.Read(tagBytes, 0, 3);

            // resize tag byte array (add an empty byte)
            Array.Resize(ref tagBytes, 4);

            // read tag value (as uint32) from tag byte array
            tag = BitConverter.ToUInt32(tagBytes, 0);

            // convert to little endian
            tag = Utilities.SwapBytes(tag) >> 8;

            // convert tag to label
            label += Convert.ToChar((((tag >> 18) & 0x3F) & 0x1F) | 64);
            label += Convert.ToChar((((tag >> 12) & 0x3F) & 0x1F) | 64);
            label += Convert.ToChar((((tag >> 6) & 0x3F) & 0x1F) | 64);
            label += Convert.ToChar(((tag & 0x3F) & 0x1F) | 64);

            // clean label
            label = Regex.Replace(label, "[^A-Z]+", "");

            return label;
        }

        /// <summary>
        /// Decodes an encoded integer.
        /// </summary>
        private ulong DecodeInteger()
        {
            // by Pedro Martins
            ulong value = (ulong)_stream.ReadByte();

            if (value >= 0x80)
            {
                value &= 0x3F;

                for (int i = 1; i < 8; i++)
                {
                    int b = _stream.ReadByte();
                    value |= (ulong)(b & 0x7F) << ((i * 7) - 1);

                    if (b < 0x80)
                    {
                        break;
                    }
                }
            }

            return value;
        }

        private string DecodeString()
        {
            string value = "";
            int length = (int)DecodeInteger() - 1;

            if (length > 0)
            {
                byte[] stringBytes = new byte[length];
                _stream.Read(stringBytes, 0, length);

                value = Encoding.ASCII.GetString(stringBytes);
            }

            // read end byte
            _stream.ReadByte();

            return value;
        }

        private List<Tdf> DecodeStruct()
        {
            List<Tdf> data = new List<Tdf>();

            while (_data[_stream.Position] != 0x00)
            {
                data.Add(ReadTdf());
            }

            // read end byte
            _stream.ReadByte();

            return data;
        }

        private ArrayList DecodeList(TdfBaseType listType, int listSize)
        {
            ArrayList list = new ArrayList();

            // weird Tdf check #1: HNET, for instance.
            if (_data[_stream.Position] == 0x02 && listType == TdfBaseType.TDF_TYPE_STRUCT)
            {
                _stream.Seek(1, SeekOrigin.Current);

                // also hack, should be 'read until 0x00' or something
                list.Add(ReadTdf());
                list.Add(ReadTdf());

                _stream.ReadByte();
            }
            else
            {
                for (int i = 0; i < listSize; i++)
                {
                    switch (listType)
                    {
                        case TdfBaseType.TDF_TYPE_INTEGER:
                            list.Add(DecodeInteger());
                            break;

                        case TdfBaseType.TDF_TYPE_STRING:
                            list.Add(DecodeString());
                            break;

                        case TdfBaseType.TDF_TYPE_STRUCT:
                            list.Add(DecodeStruct());
                            break;

                        default:
                            break;
                    }
                }
            }

            return list;
        }

        private TdfInteger DecodeTdfInteger(string label)
        {
            ulong value = DecodeInteger();

            return new TdfInteger(label, value);
        }

        private TdfString DecodeTdfString(string label)
        {
            string value = DecodeString();

            return new TdfString(label, value);
        }

        private TdfBlob DecodeTdfBlob(string label)
        {
            int length = (int)DecodeInteger();
            byte[] data = new byte[length];

            for (int i = 0; i < length; i++)
            {
                data[i] = (byte)_stream.ReadByte();
            }

            return new TdfBlob(label, data);
        }

        private TdfStruct DecodeTdfStruct(string label)
        {
            List<Tdf> data = DecodeStruct();

            return new TdfStruct(label, data);
        }

        private TdfList DecodeTdfList(string label)
        {
            // read list type
            byte listType = (byte)_stream.ReadByte();

            // read list size
            int listSize = _stream.ReadByte();

            // read list
            ArrayList data = DecodeList((TdfBaseType)listType, listSize);

            TdfList list = new TdfList(label, (TdfBaseType)listType, data);

            return list;
        }

        private TdfMap DecodeTdfMap(string label)
        {
            // read list types
            TdfBaseType listType1 = (TdfBaseType)_stream.ReadByte();
            TdfBaseType listType2 = (TdfBaseType)_stream.ReadByte();

            // read list size
            int listSize = _stream.ReadByte();

            TdfMap map = new TdfMap(label, listType1, listType2, new Dictionary<object, object>());

            // read list
            Func<TdfBaseType, Object> readListItem = (type) =>
            {
                Object item = null;

                switch (type)
                {
                    case TdfBaseType.TDF_TYPE_INTEGER:
                        item = DecodeInteger();
                        break;

                    case TdfBaseType.TDF_TYPE_STRING:
                        item = DecodeString();
                        break;

                    case TdfBaseType.TDF_TYPE_STRUCT:
                        item = DecodeStruct();
                        break;

                    default:
                        Log.Warn(string.Format("Unknown list item type: {0}.", type));
                        break;
                }

                return item;
            };

            for (int i = 0; i < listSize; i++)
            {
                Object key = readListItem(listType1);
                Object value = readListItem(listType2);

                if (key != null | value != null)
                {
                    map.map.Add(key, value);
                }
            }

            return map;
        }

        private TdfUnion DecodeTdfUnion(string label)
        {
            NetworkAddressMember activeMember = (NetworkAddressMember)_stream.ReadByte();

            TdfUnion union = new TdfUnion(label, activeMember, new List<Tdf> { });

            // if active member is not set then there are no data members
            if (activeMember != NetworkAddressMember.MEMBER_UNSET)
            {
                union.data.Add(ReadTdf());
            }

            return union;
        }

        private TdfVector2 DecodeTdfVector2(string label)
        {
            ulong value1 = DecodeInteger();
            ulong value2 = DecodeInteger();

            return new TdfVector2(label, value1, value2);
        }

        private TdfVector3 DecodeTdfVector3(string label)
        {
            ulong value1 = DecodeInteger();
            ulong value2 = DecodeInteger();
            ulong value3 = DecodeInteger();

            return new TdfVector3(label, value1, value2, value3);
        }

        private Tdf ReadTdf()
        {
            Tdf tdf = null;

            string label = ReadLabel();
            TdfBaseType type = (TdfBaseType)_stream.ReadByte();

            switch (type)
            {
                case TdfBaseType.TDF_TYPE_INTEGER:
                    tdf = DecodeTdfInteger(label);
                    break;

                case TdfBaseType.TDF_TYPE_STRING:
                    tdf = DecodeTdfString(label);
                    break;

                case TdfBaseType.TDF_TYPE_BINARY:
                    tdf = DecodeTdfBlob(label);
                    break;

                case TdfBaseType.TDF_TYPE_STRUCT:
                    tdf = DecodeTdfStruct(label);
                    break;

                case TdfBaseType.TDF_TYPE_LIST:
                    tdf = DecodeTdfList(label);
                    break;

                case TdfBaseType.TDF_TYPE_MAP:
                    tdf = DecodeTdfMap(label);
                    break;

                case TdfBaseType.TDF_TYPE_UNION:
                    tdf = DecodeTdfUnion(label);
                    break;

                case TdfBaseType.TDF_TYPE_BLAZE_OBJECT_TYPE:
                    tdf = DecodeTdfVector2(label);
                    break;

                case TdfBaseType.TDF_TYPE_BLAZE_OBJECT_ID:
                    tdf = DecodeTdfVector3(label);
                    break;

                default:
                    Log.Warn(string.Format("Unknown Tdf type: {0}.", type));
                    break;
            }

            return tdf;
        }

        public Dictionary<string, Tdf> Decode()
        {
            while (_stream.Position != _stream.Length)
            {
                Tdf tdf = ReadTdf();

                if (tdf != null)
                {
                    _result.Add(tdf.label, tdf);
                }
            }

            /*
            string result = "";

            _result.ForEach(delegate(Tdf item)
            {
                result += string.Format("{0}({1}), ", item.label, item.type);

                Console.WriteLine(string.Format("{0}: {1}", item.label, item.type));
            });

            //Console.WriteLine(result);
            */

            return _result;
        }
    }
}