using System;
using System.Linq;

namespace BlazeServer
{
    public enum MessageType
    {
        MESSAGE = 0x0,
        REPLY = 0x1,
        NOTIFICATION = 0x2,
        ERROR_REPLY = 0x3
    };

    public struct Packet
    {
        public int headerSize;
        public Int32 componentId;
        public Int32 commandId;
        public int errorCode;
        public UInt32 msgType;
        public int msgNum;
        public int payloadSize;
        public byte[] payload;
    }

    public class FireFrame
    {
        private byte[] mFrame;

        public FireFrame(byte[] data)
        {
            mFrame = data;

            Log.Debug(string.Format("{0} {1} {2} {3} {4} {5}",
                GetComponent(),
                GetCommand(),
                GetSize(),
                GetHeaderSize(),
                GetMsgNum(),
                MessageTypeToString((MessageType)GetMsgType())));
        }

        /* public Packet Decode()
        {
            Packet packet = new Packet();

            // decode header size
            packet.headerSize = (((mFrame[9] >> 3) & 4) + ((mFrame[9] >> 3) & 2) + 12);

            // decode header
            packet.componentId = GetComponent();
            packet.commandId = GetCommand();
            packet.errorCode = GetErrorCode();
            packet.msgType = GetMsgType();
            packet.msgNum = GetMsgNum();

            // decode payload size
            packet.payloadSize = mFrame[1] | (mFrame[0] << 8);

            if (Convert.ToBoolean(mFrame[9] & 0x10))
            {
                packet.payloadSize |= (mFrame[13] | (mFrame[12] << 8) >> 16);
            }

            // decode payload
            packet.payload = mFrame.Skip(packet.headerSize).Take(packet.payloadSize).ToArray();

            return packet;
        } */

        public static String MessageTypeToString(MessageType type)
        {
            String result = "";

            switch (type)
            {
                case MessageType.MESSAGE:
                    result = "MESSAGE";
                    break;

                case MessageType.REPLY:
                    result = "REPLY";
                    break;

                case MessageType.NOTIFICATION:
                    result = "NOTIFICATION";
                    break;

                case MessageType.ERROR_REPLY:
                    result = "ERROR_REPLY";
                    break;

                default:
                    result = "UNKNOWN";
                    break;
            }

            return result;
        }

        public Int32 GetSize()
        {
            return mFrame[1] | (mFrame[0] << 8);
        }

        public Int32 GetMsgNum()
        {
            return mFrame[11] | ((mFrame[10] | ((mFrame[9] & 0xF) << 8)) << 8);
        }

        public UInt32 GetHeaderSize()
        {
            return (UInt32)(((mFrame[9] >> 3) & 4) + ((mFrame[9] >> 3) & 2) + 12);
        }

        public Int32 GetComponent()
        {
            return mFrame[3] | (mFrame[2] << 8);
        }

        public Int32 GetCommand()
        {
            return mFrame[5] | (mFrame[4] << 8);
        }

        public Int32 GetErrorCode()
        {
            return mFrame[7] | (mFrame[6] << 8);
        }

        public UInt32 GetMsgType()
        {
            return (UInt32)(mFrame[8] >> 4);
        }

        public Int32 GetUserIndex()
        {
            return mFrame[8] & 0xF;
        }

        public UInt32 GetOptions()
        {
            return (UInt32)(mFrame[9] >> 4);
        }

	    public UInt32 HasContext()
	    {
		    return (UInt32)((mFrame[9] >> 5) & 1);
	    }

	    public UInt32 IsJumboFrame()
	    {
            return (UInt32)((mFrame[9] >> 4) & 1);
	    }
    }
}