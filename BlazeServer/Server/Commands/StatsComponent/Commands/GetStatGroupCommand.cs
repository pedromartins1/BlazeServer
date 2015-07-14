using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class GetStatGroupCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            var client = ClientManager.GetClient(clientId);

            string player_awards = "player_awards";
            string player_awards2 = "player_awards2";
            string player_weapons1 = "player_weapons1";
            string player_statcategory = "player_statcategory";
            string player_core = "player_core";
            string coopplayer_coop = "coopplayer_coop";
            string player_reset = "player_reset";

            string[] catg = new string[1433];

            for (int j = 0; j < 993; j++)
                catg[j] = player_awards;
            for (int k = 993; k < 1166; k++)
                catg[k] = player_awards2;
            for (int l = 1166; l < 1296; l++)
                catg[l] = player_weapons1;
            for (int m = 1296; m < 1406; m++)
                catg[m] = player_statcategory;
            for (int n = 1406; n < 1427; n++)
                catg[n] = player_core;
            catg[1427] = coopplayer_coop;
            catg[1428] = player_core;
            catg[1429] = player_core;
            catg[1430] = player_core;
            catg[1431] = player_statcategory;
            catg[1432] = player_reset;

            TdfList stat = new TdfList("STAT", TdfBaseType.TDF_TYPE_STRUCT, new ArrayList { });

            for (int i = 0; i < 1433; i++)
            {
                stat.list.Add(new List<Tdf>
                {
                    new TdfString("CATG", catg[i]),
                    new TdfString("DFLT", "0.00"),
                    new TdfInteger("DRVD", 0x0),
                    new TdfString("FRMT", "%.2f"),
                    new TdfString("KIND", ""),
                    new TdfString("LDSC", ""),
                    new TdfString("META", ""),
                    new TdfString("NAME", Utilities.GetStatName(i)),
                    new TdfString("SDSC", ""),
                    new TdfInteger("TYPE", 0x1)
                });
            }

            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfString("CNAM", player_awards),
                new TdfString("DESC", "player_mpdefault2"), // TODO: fetch name from decoded data
                new TdfVector2("ETYP", 30722, 1),
                new TdfString("META", ""),
                new TdfString("NAME", "player_mpdefault2"),
                stat
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.STATS,
                commandId = 0x4,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
