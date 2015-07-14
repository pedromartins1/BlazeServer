using System;
using System.IO;
using System.Linq;

namespace BlazeServer
{
    public class ProtoFire
    {
        private Packet _packet;
        private MemoryStream _stream;

        public ProtoFire(Packet packet)
        {
            _packet = packet;
            _stream = new MemoryStream();
        }

        public byte[] Encode()
        {
            // encode payload size
            _stream.WriteByte((byte)((_packet.payloadSize & 0xFFFF) >> 8));
            _stream.WriteByte((byte)((byte)_packet.payloadSize & 0xFF));

            // encode header
            _stream.WriteByte((byte)(((ushort)_packet.componentId >> 8) & 0xFF));
            _stream.WriteByte((byte)((ushort)_packet.componentId & 0xFF));

            _stream.WriteByte((byte)(((ushort)_packet.commandId >> 8) & 0xFF));
            _stream.WriteByte((byte)((ushort)_packet.commandId & 0xFF));

            _stream.WriteByte((byte)(((byte)_packet.errorCode >> 8) & 0xFF));
            _stream.WriteByte((byte)((byte)_packet.errorCode & 0xFF));

            _stream.WriteByte((byte)((byte)_packet.msgType * 16));
            _stream.WriteByte((byte)(((byte)_packet.msgNum >> 16) & 0xF));

            _stream.WriteByte((byte)((byte)_packet.msgNum >> 8));
            _stream.WriteByte((byte)((byte)_packet.msgNum & 0xFF));

            if (_packet.payload != null && _packet.payloadSize > 0)
            {
                _stream.Write(_packet.payload, 0, _packet.payloadSize);
            }

            byte[] buffer = _stream.GetBuffer();
            int position = (int)_stream.Position;

            return buffer.Take(position).ToArray();
        }
    }
}