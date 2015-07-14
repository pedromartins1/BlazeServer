using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class PlayerJoinCompletedNotification
    {
        public static void Notify(ulong gameId, ulong personaId, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfInteger("GID", gameId),
                new TdfInteger("PID", personaId)
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x1E,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
