using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class GetStatsAsyncNotification
    {
        public static void Notify(ulong clientId, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            var client = ClientManager.GetClient(clientId);
            var game = Database.GetGameByID(client.gameId);

            ArrayList statValues = new ArrayList();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfString("GRNM", "player_mpdefault2"),
                new TdfString("KEY", "No_Scope_Defined"),
                new TdfInteger("LAST", 1),
                new TdfStruct("STS", new List<Tdf>
                {
                    new TdfList("STAT", TdfBaseType.TDF_TYPE_STRUCT, new ArrayList
                    {
                        new List<Tdf>
                        {
                            new TdfInteger("EID", (ulong)client.entityIds[0]),
                            new TdfVector2("ETYP", 0x7802, 0x1),
                            new TdfInteger("POFF", 0),
                            new TdfList("STAT", TdfBaseType.TDF_TYPE_STRING, statValues)
                        }
                    })
                }),
                new TdfInteger("VID", client.viewId)
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.STATS,
                commandId = 0x32,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
