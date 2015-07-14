using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class GameCapacityChangeNotification
    {
        public static void Notify(ulong gameId, ArrayList capacity, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfList("CAP", TdfBaseType.TDF_TYPE_INTEGER, capacity),
                new TdfInteger("GID", gameId),
                new TdfInteger("TCAP", 0)
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x6F,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
