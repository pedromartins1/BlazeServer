using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class SilentLoginCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfString auth = (TdfString)data["AUTH"];
            ClientManager.UpdateClientAuthToken(clientId, auth.value);

            TdfInteger pid = (TdfInteger)data["PID"];

            var user = Database.GetUser(pid.value);

            if (user.mail != null)
            {
                ClientManager.SetClientUser(clientId, user);
            }
            else
            {
                Log.Warn(string.Format("Could not find user by persona ID {0}.", pid.value));
            }

            Log.Info(string.Format("Performing silent login with persona {0} for user {1}.", pid.value, user.id));

            var persona = Database.GetPersona(pid.value);

            if (persona.name != null)
            {
                //Log.Info(string.Format("User {0} logging in to persona {1} with auth token {2}.", user.id, persona.name, auth.value));
                ClientManager.SetClientPersona(clientId, persona);
            }
            else
            {
                Log.Warn(string.Format("Could not find persona {0} for user {1}.", pid.value, user.id));
            }

            // not sure what is this type for
            //TdfInteger type = (TdfInteger)data.Find(tdf => tdf.label == "TYPE");

            var client = ClientManager.GetClient(clientId);

            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfInteger("AGUP", 0),
                new TdfString("LDHT", ""),
                new TdfInteger("NTOS", 0),
                new TdfString("PCTK", "fa1a26c1-d934-422a-a6ba-ed92614f7d87"),
                new TdfString("PRIV", ""),
                new TdfStruct("SESS", new List<Tdf>
                {
                    new TdfInteger("BUID", client.persona.id),
                    new TdfInteger("FRST", 0),
                    new TdfString("KEY", "some_key"),
                    new TdfInteger("LLOG", Utilities.GetUnixTime()),
                    new TdfString("MAIL", client.user.mail), // TODO: get mail for client
                    new TdfStruct("PDTL", new List<Tdf>
                    {
                        new TdfString("DSNM", client.persona.name), // persona display name
                        new TdfInteger("LAST", Utilities.GetUnixTime()), // time of last persona authentication
                        new TdfInteger("PID", client.persona.id), // persona ID
                        new TdfInteger("STAS", 0), // should be ACTIVE(2)?
                        new TdfInteger("XREF", 0),
                        new TdfInteger("XTYP", (ulong)ExternalRefType.BLAZE_EXTERNAL_REF_TYPE_UNKNOWN) // this is actually a TdfMin
                    }),
                    new TdfInteger("UID", clientId)
                }),
                new TdfInteger("SPAM", 0),
                new TdfString("THST", ""),
                new TdfString("TSUI", ""),
                new TdfString("TURI", "")
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.AUTHENTICATION,
                commandId = 0x32,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
