using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class UserAddedNotification
    {
        public static void Notify(ulong clientId, SslStream stream, bool joining = false)
        {
            var client = ClientManager.GetClient(clientId);

            TdfEncoder encoder = new TdfEncoder();

            // TODO: check if this is correct
            ulong longid = client.persona.id;
            string pname = client.persona.name;

            if (client.type == ClientType.CLIENT_TYPE_GAMEPLAY_USER && joining)
            {
                longid = client.gameId;
                pname = "bf3-server-pc";

                //user.data.Add(new TdfBlob("EXBB", new byte[] { }));
            }

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfStruct("DATA", new List<Tdf>
                {
                    new TdfUnion("ADDR", NetworkAddressMember.MEMBER_UNSET, new List<Tdf> { }),
                    new TdfString("BPS", ""),
                    new TdfString("CTY", ""),
                    new TdfMap("DMAP", TdfBaseType.TDF_TYPE_INTEGER, TdfBaseType.TDF_TYPE_INTEGER, new Dictionary<object, object>
                    {
                        { (ulong)0x70001, (ulong)55 },
                        { (ulong)0x70002, (ulong)707 }
                    }),
                    new TdfInteger("HWFG", 0),
                    new TdfStruct("QDAT", new List<Tdf>
                    {
                        new TdfInteger("DBPS", 0),
                        new TdfInteger("NATT", (ulong)NatType.NAT_TYPE_OPEN), // TdfMin
                        new TdfInteger("UBPS", 0)
                    }),

                    new TdfInteger("UATT", 0)
                    //new TdfList("ULST", 9)
                }),
                new TdfStruct("USER", new List<Tdf>
                {
                    new TdfInteger("AID", clientId),
                    new TdfInteger("ALOC", client.localization),
                    new TdfInteger("ID", longid),
                    new TdfString("NAME", pname)
                })
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.USERSESSIONS,
                commandId = 0x2,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
