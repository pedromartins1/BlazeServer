using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class LoginPersonaCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfString pnam = (TdfString)data["PNAM"];

            var client = ClientManager.GetClient(clientId);

            Log.Info(string.Format("User {0} logging in to persona {1}.", client.user.id, pnam.value));

            TdfEncoder encoder = new TdfEncoder();
            encoder.WriteTdf(new TdfInteger("BUID", client.persona.id));
            encoder.WriteTdf(new TdfInteger("FRST", 0));
            encoder.WriteTdf(new TdfString("KEY", "10c38a80_a223eb9d6c2c199db1885856a297055d"));
            encoder.WriteTdf(new TdfInteger("LLOG", Utilities.GetUnixTime()));
            encoder.WriteTdf(new TdfString("MAIL", client.user.mail));

            encoder.WriteTdf(new TdfStruct("PDTL", new List<Tdf>
            {
                new TdfString("DSNM", client.persona.name), // persona display name
                new TdfInteger("LAST", Utilities.GetUnixTime()), // time of last persona authentication
                new TdfInteger("PID", 0), // persona ID
                new TdfInteger("STAS", 2), // ACTIVE
                new TdfInteger("XREF", 0),
                new TdfInteger("XTYP", (ulong)ExternalRefType.BLAZE_EXTERNAL_REF_TYPE_UNKNOWN) // this is actually a TdfMin
            }));

            encoder.WriteTdf(new TdfInteger("UID", clientId)); // Blaze user ID

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.AUTHENTICATION,
                commandId = 0x6E,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
