using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class LoginCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfString mail = (TdfString)data["MAIL"];
            TdfString pass = (TdfString)data["PASS"];

            var client = ClientManager.GetClient(clientId);

            var user = Database.GetUser(mail.value, pass.value);

            if (user.id != 0)
            {
                Log.Info(string.Format("User {0} logging in with mail {1}.", user.id, user.mail));
            }
            else
            {
                Log.Error(string.Format("Could not find user for user {0}.", user.id));
            }

            ClientManager.SetClientUser(clientId, user);

            var persona = Database.GetPersona(user.personaId);

            if (persona.name != null)
            {
                Log.Debug(string.Format("Found persona '{0}' for user {1}.", persona.name, user.id));
            }
            else
            {
                Log.Error(string.Format("Could not find persona for user {0}.", user.id));
            }

            ClientManager.SetClientPersona(clientId, persona);

            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfString("LDHT", ""),
                new TdfInteger("NTOS", 0),
                new TdfString("PCTK", "NnQWcHUvUoE4gBscrqJNMx6QzKKrVnnbaZBnD4ZY9kEzKg0cBlW0TrCWil1I8GokLs9p0h_stN5UvWYaS3IrQEK6qvjFwY59k6R_NYIKCp4"),
                new TdfList("PLST", TdfBaseType.TDF_TYPE_STRUCT, new ArrayList
                {
                    new List<Tdf>
                    {
                        new TdfString("DSNM", persona.name),
                        new TdfInteger("LAST", Utilities.GetUnixTime()),
                        new TdfInteger("PID", persona.id),
                        new TdfInteger("STAS", 2), // 'ACTIVE'
                        new TdfInteger("XREF", 0),
                        new TdfInteger("XTYP", (ulong)ExternalRefType.BLAZE_EXTERNAL_REF_TYPE_UNKNOWN)
                    }
                }),
                new TdfString("PRIV", ""),
                new TdfString("SKEY", "10c38a80_a223eb9d6c2c199db1885856a297055d"),
                new TdfInteger("SPAM", 1),
                new TdfString("THST", ""),
                new TdfString("TSUI", ""),
                new TdfString("TURI", ""),
                new TdfInteger("UID", clientId)
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.AUTHENTICATION,
                commandId = 0x28,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
