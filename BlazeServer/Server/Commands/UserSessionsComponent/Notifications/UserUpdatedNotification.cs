using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class UserUpdatedNotification
    {
        public static void Notify(ulong personaId, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfInteger("FLGS", 3), // TdfMin
                new TdfInteger("ID", personaId) // persona ID
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.USERSESSIONS,
                commandId = 0x5,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
