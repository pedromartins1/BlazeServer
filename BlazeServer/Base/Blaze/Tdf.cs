using System;
using System.Collections;
using System.Collections.Generic;

namespace BlazeServer
{
    public enum TdfBaseType : byte
	{
		TDF_TYPE_MIN = 0x0,
		TDF_TYPE_INTEGER = 0x0,
		TDF_TYPE_STRING = 0x1,
		TDF_TYPE_BINARY = 0x2,
		TDF_TYPE_STRUCT = 0x3,
		TDF_TYPE_LIST = 0x4,
		TDF_TYPE_MAP = 0x5,
		TDF_TYPE_UNION = 0x6,
		TDF_TYPE_VARIABLE = 0x7,
		TDF_TYPE_BLAZE_OBJECT_TYPE = 0x8,
		TDF_TYPE_BLAZE_OBJECT_ID = 0x9,
		TDF_TYPE_FLOAT = 0xA,
		TDF_TYPE_TIMEVALUE = 0xB,
		TDF_TYPE_MAX = 0xC
	};

    public class Tdf
    {
        public string label;
        public TdfBaseType type;
    }

    public class TdfMin : Tdf
    {
        public ushort value;

        public TdfMin(string label, ushort value)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_MIN;

            this.value = value;
        }
    }

    public class TdfInteger : Tdf
    {
        public ulong value;

        public TdfInteger(string label, ulong value)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_INTEGER;

            this.value = value;
        }
    }

    public class TdfString : Tdf
    {
        public string value;

        public TdfString(string label, string value)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_STRING;

            this.value = value;
        }
    }

    public class TdfBlob : Tdf
    {
        public byte[] data;

        public TdfBlob(string label, byte[] data)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_BINARY;

            this.data = data;
        }
    }

    public class TdfStruct : Tdf
    {
        public List<Tdf> data;

        public TdfStruct(string label, List<Tdf> data)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_STRUCT;

            this.data = data;
        }
    }

    public class TdfList : Tdf
    {
        public TdfBaseType listType;
        public ArrayList list;

        public bool stub;

        public TdfList(string label, TdfBaseType listType, ArrayList list, bool stub = false)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_LIST;

            this.listType = listType;
            this.list = list;

            this.stub = stub;
        }
    }

    public class TdfMap : Tdf
    {
        public TdfBaseType type1;
        public TdfBaseType type2;

        public Dictionary<object, object> map;

        public TdfMap(string label, TdfBaseType type1, TdfBaseType type2, Dictionary<object, object> map)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_MAP;

            this.type1 = type1;
            this.type2 = type2;

            this.map = map;
        }
    }

    public class TdfUnion : Tdf
    {
        public NetworkAddressMember activeMember;
        public List<Tdf> data;

        public TdfUnion(string label, NetworkAddressMember value, List<Tdf> data)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_UNION;

            this.activeMember = value;
            this.data = data;
        }
    }

    public class TdfIntegerList : Tdf
    {
        public List<ulong> list;

        public TdfIntegerList(string label, List<ulong> list)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_VARIABLE;

            this.list = list;
        }
    }

    public class TdfVector2 : Tdf
    {
        public ulong value1;
        public ulong value2;

        public TdfVector2(string label, ulong value1, ulong value2)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_BLAZE_OBJECT_TYPE;

            this.value1 = value1;
            this.value2 = value2;
        }
    }

    public class TdfVector3 : Tdf
    {
        public ulong value1;
        public ulong value2;
        public ulong value3;

        public TdfVector3(string label, ulong value1, ulong value2, ulong value3)
        {
            this.label = label;
            this.type = TdfBaseType.TDF_TYPE_BLAZE_OBJECT_ID;

            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }
    }
}