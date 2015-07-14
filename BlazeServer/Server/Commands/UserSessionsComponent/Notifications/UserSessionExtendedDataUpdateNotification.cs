using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class UserSessionExtendedDataUpdateNotification
    {
        public static void Notify(ulong clientId, SslStream stream, bool ulst = false)
        {
            var client = ClientManager.GetClient(clientId);

            TdfEncoder encoder = new TdfEncoder();

            TdfList pslm = new TdfList("PSLM", TdfBaseType.TDF_TYPE_INTEGER, new ArrayList { });
            pslm.list.AddRange(new ulong[] { 268374015, 268374015, 268374015, 268374015, 268374015 });

            TdfStruct data = new TdfStruct("DATA", new List<Tdf>
            {
                new TdfUnion("ADDR", NetworkAddressMember.MEMBER_IPPAIRADDRESS, new List<Tdf>
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
                new TdfString("BPS", "ams"),
                new TdfString("CTY", ""),
                new TdfInteger("HWFG", 0),
                pslm,
                new TdfInteger("UATT", 0)
            });

            if (ulst == true)
            {
                data.data.Add(new TdfList("ULST", TdfBaseType.TDF_TYPE_BLAZE_OBJECT_ID, new ArrayList
                {
                    new TdfVector3("0", 0x4, 0x1, client.gameId)
                }));
            }

            encoder.WriteTdf(new List<Tdf>
            {
                data,
                new TdfInteger("USID", client.persona.id)
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.USERSESSIONS,
                commandId = 0x1,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
