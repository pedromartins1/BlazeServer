using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class PlayerJoiningNotification
    {
        public static void Notify(ulong clientId, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            var client = ClientManager.GetClient(clientId);
            var game = Database.GetGameByID(client.gameId);

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfInteger("GID", client.gameId),
                new TdfStruct("PDAT", new List<Tdf>
                {
                    new TdfInteger("EXID", 0),
                    new TdfInteger("GID", client.gameId),
                    new TdfInteger("LOC", client.localization),
                    new TdfString("NAME", client.persona.name),
                    new TdfMap("PATT", TdfBaseType.TDF_TYPE_STRING, TdfBaseType.TDF_TYPE_STRING, new Dictionary<object, object>
                    {
                        { "Premium", "True" }
                    }),
                    new TdfInteger("PID", client.persona.id),
                    new TdfUnion("PNET", NetworkAddressMember.MEMBER_IPPAIRADDRESS, new List<Tdf>
                    {
                        new TdfStruct("VALU", new List<Tdf>
                        {
                            new TdfStruct("EXIP", new List<Tdf>
                            {
                                new TdfInteger("IP", client.externalNetworkInfo.ip),
                                new TdfInteger("PORT", client.externalNetworkInfo.port)
                            }),
                            new TdfStruct("INIP", new List<Tdf>
                            {
                                new TdfInteger("IP", client.internalNetworkInfo.ip),
                                new TdfInteger("PORT", client.internalNetworkInfo.port)
                            })
                        })
                    }),
                    new TdfInteger("SID", 1),
                    new TdfInteger("SLOT", 0),
                    new TdfInteger("STAT", 0),
                    new TdfInteger("TIDX", 65535),
                    new TdfInteger("TIME", 0),
                    new TdfVector3("UGID", 0, 0, 0),
                    new TdfInteger("UID", clientId)
                })
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x15,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
