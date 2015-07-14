using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlazeServer
{
    public class TdfEncoder
    {
        private MemoryStream _stream;

        public TdfEncoder()
        {
            _stream = new MemoryStream();
        }

        /// <summary>
        /// Encodes a label to tag.
        /// </summary>
        /// <param name="label">Label to write.</param>
        private void WriteLabel(string label)
        {
            // by Pedro Martins
            int tag = 0;

            for (int i = 0; i < label.Length; i++)
            {
                tag |= (0x20 | (label[i] & 0x1F)) << (3 - i) * 6;
            }

            uint sTag = Utilities.SwapBytes(Convert.ToUInt32(tag)) >> 8;

            // hackhackhack
            // FIXME
            MemoryStream hehStream = new MemoryStream();
            BinaryWriter hehWriter = new BinaryWriter(hehStream);
            hehWriter.Write(sTag);

            _stream.Write(hehStream.GetBuffer().Take(3).ToArray(), 0, 3);
        }

        private void WriteInteger(ulong value)
        {
            if (value < 0x40)
            {
                _stream.WriteByte((byte)(value & 0xFF));
            }
            else
            {
                _stream.WriteByte((byte)((value & 0x3F) | 0x80));
                ulong currshift = value >> 6;

                while (currshift >= 0x80)
                {
                    _stream.WriteByte((byte)((currshift & 0x7F) | 0x80));
                    currshift >>= 7;
                }

                _stream.WriteByte((byte)currshift);
            }
        }

        private void WriteString(string value)
        {
            byte[] stringBytes = Encoding.ASCII.GetBytes(value);
            uint length = (uint)value.Length;

            // write string length
            WriteInteger(length + 1);

            // write string
            _stream.Write(stringBytes, 0, (int)length);

            // write end byte
            _stream.WriteByte(0);
        }

        private void WriteBlob(byte[] data)
        {
            uint length = (uint)data.Length;

            // write lenght
            WriteInteger(length);

            // write data
            for (int i = 0; i < length; i++)
            {
                _stream.WriteByte(data[i]);
            }
        }

        private void WriteStruct(List<Tdf> data)
        {
            // write struct items
            data.ForEach(delegate(Tdf item)
            {
                WriteTdf(item);
            });

            // write end byte
            _stream.WriteByte(0);
        }

        /// <summary>
        /// Writes a list to the stream.
        /// </summary>
        /// <param name="type">List type.</param>
        /// <param name="list">List to write.</param>
        private void WriteList(TdfBaseType type, ArrayList list)
        {
            foreach (Object obj in list)
            {
                switch (type)
                {
                    case TdfBaseType.TDF_TYPE_INTEGER:
                        WriteInteger((ulong)obj);
                        break;

                    case TdfBaseType.TDF_TYPE_STRING:
                        WriteString((string)obj);
                        break;

                    case TdfBaseType.TDF_TYPE_STRUCT:
                        WriteStruct((List<Tdf>)obj);
                        break;

                    case TdfBaseType.TDF_TYPE_BLAZE_OBJECT_ID:
                        WriteTdfVector3((TdfVector3)obj);
                        break;

                    default:
                        Log.Warn(string.Format("Unknown list type: {0}.", type));
                        break;
                }
            }
        }

        private void WriteTdfMin(TdfMin tdf)
        {
            // write value
            _stream.WriteByte((byte)tdf.value);
        }

        private void WriteTdfInteger(TdfInteger tdf)
        {
            // write value
            WriteInteger(tdf.value);
        }

        private void WriteTdfList(TdfList tdf)
        {
            // write list type
            _stream.WriteByte((byte)tdf.listType);

            // write list size
            _stream.WriteByte((byte)tdf.list.Count);

            if (tdf.stub)
            {
                _stream.WriteByte(2);
            }

            // write list
            WriteList(tdf.listType, tdf.list);
        }

        private void WriteTdfMap(TdfMap tdf)
        {
            // write list types
            _stream.WriteByte((byte)tdf.type1);
            _stream.WriteByte((byte)tdf.type2);

            // write list size
            _stream.WriteByte((byte)tdf.map.Count);

            // write map
            Action<TdfBaseType, Object> writeListItem = (type, item) =>
            {
                switch (type)
                {
                    case TdfBaseType.TDF_TYPE_INTEGER:
                        WriteInteger((ulong)item);
                        break;

                    case TdfBaseType.TDF_TYPE_STRING:
                        WriteString((string)item);
                        break;

                    case TdfBaseType.TDF_TYPE_STRUCT:
                        WriteStruct((List<Tdf>)item);
                        break;

                    default:
                        Log.Warn(string.Format("Unknown list item type: {0}.", type));
                        break;
                }
            };

            foreach (var item in tdf.map)
            {
                writeListItem(tdf.type1, item.Key);
                writeListItem(tdf.type2, item.Value);
            }
        }

        private void WriteTdfUnion(TdfUnion tdf)
        {
            // write active member
            _stream.WriteByte((byte)tdf.activeMember);

            if (tdf.activeMember != NetworkAddressMember.MEMBER_UNSET)
            {
                tdf.data.ForEach(delegate(Tdf item)
                {
                    WriteTdf(item);
                });
            }
        }

        private void WriteTdfIntegerList(TdfIntegerList tdf)
        {
            // write list size
            _stream.WriteByte((byte)tdf.list.Count);

            // write list
            foreach (ulong l in tdf.list)
            {
                WriteInteger(l);
            }
        }

        private void WriteTdfVector2(TdfVector2 tdf)
        {
            WriteInteger(tdf.value1);
            WriteInteger(tdf.value2);
        }

        private void WriteTdfVector3(TdfVector3 tdf)
        {
            WriteInteger(tdf.value1);
            WriteInteger(tdf.value2);
            WriteInteger(tdf.value3);
        }

        /// <summary>
        /// Encodes a Tdf.
        /// </summary>
        /// <param name="tdf">The input Tdf.</param>
        public void WriteTdf(Tdf tdf)
        {
            // write label
            WriteLabel(tdf.label);

            // write type
            _stream.WriteByte((byte)tdf.type);

            switch (tdf.type)
            {
                /* case TdfBaseType.TDF_TYPE_MIN:
                    WriteTdfMin((TdfMin)tdf);
                    break; */

                case TdfBaseType.TDF_TYPE_INTEGER:
                    WriteTdfInteger((TdfInteger)tdf);
                    break;

                case TdfBaseType.TDF_TYPE_STRING:
                    WriteString(((TdfString)tdf).value);
                    break;

                case TdfBaseType.TDF_TYPE_BINARY:
                    WriteBlob(((TdfBlob)tdf).data);
                    break;

                case TdfBaseType.TDF_TYPE_STRUCT:
                    WriteStruct(((TdfStruct)tdf).data);
                    break;

                case TdfBaseType.TDF_TYPE_LIST:
                    WriteTdfList((TdfList)tdf);
                    break;

                case TdfBaseType.TDF_TYPE_MAP:
                    WriteTdfMap((TdfMap)tdf);
                    break;

                case TdfBaseType.TDF_TYPE_UNION:
                    WriteTdfUnion((TdfUnion)tdf);
                    break;

                case TdfBaseType.TDF_TYPE_VARIABLE:
                    WriteTdfIntegerList((TdfIntegerList)tdf);
                    break;

                case TdfBaseType.TDF_TYPE_BLAZE_OBJECT_TYPE:
                    WriteTdfVector2((TdfVector2)tdf);
                    break;

                case TdfBaseType.TDF_TYPE_BLAZE_OBJECT_ID:
                    WriteTdfVector3((TdfVector3)tdf);
                    break;

                default:
                    Log.Warn(string.Format("Unknown Tdf type: {0}.", tdf.type));
                    break;
            }
        }

        public void WriteTdf(List<Tdf> tdfs)
        {
            tdfs.ForEach(delegate(Tdf tdf)
            {
                WriteTdf(tdf);
            });
        }

        public byte[] Encode()
        {
            byte[] buffer = _stream.GetBuffer();
            int position = (int)_stream.Position;

            return buffer.Take(position).ToArray();
        }
    }
}