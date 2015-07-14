using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class PlayerRemovedNotification
    {
        public static void Notify(ulong gameId, ulong personaId, PlayerRemovedReason reason, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfInteger("CNTX", 0),
                new TdfInteger("GID", gameId),
                new TdfInteger("PID", personaId),
                new TdfInteger("REAS", (ulong)reason)
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x28,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
