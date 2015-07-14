using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class PingCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            var time = Utilities.GetUnixTime();

            ClientManager.UpdateClientLastSeen(clientId, time);

            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfInteger("STIM", time)
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.UTIL,
                commandId = 0x2,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
