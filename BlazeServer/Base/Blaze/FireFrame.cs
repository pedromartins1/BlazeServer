using System;
using System.Linq;

namespace BlazeServer
{
    public class FireFrame
    {
        private byte[] _data;

        public FireFrame(byte[] data)
        {
            _data = data;
        }

        public Packet Decode()
        {
            Packet packet = new Packet();

            // decode header size
            packet.headerSize = (((_data[9] >> 3) & 4) + ((_data[9] >> 3) & 2) + 12);

            // decode header
            packet.componentId = (Component)(_data[3] | (_data[2] << 8));
            packet.commandId = (ushort)(_data[5] | (_data[4] << 8));
            packet.errorCode = _data[7] | (_data[6] << 8);
            packet.msgType = (MessageType)((_data[8] >> 4) << 16);
            packet.msgNum = _data[11] | ((_data[10] | ((_data[9] & 0xF) << 8)) << 8);

            // decode payload size
            packet.payloadSize = _data[1] | (_data[0] << 8);

            if (Convert.ToBoolean(_data[9] & 0x10))
            {
                packet.payloadSize |= (_data[13] | (_data[12] << 8) >> 16);
            }

            // decode payload
            packet.payload = _data.Skip(packet.headerSize).Take(packet.payloadSize).ToArray();

            return packet;
        }
    }
}