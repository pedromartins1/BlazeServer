using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class ListUserEntitlements2Command
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);
            TdfString etag = (TdfString)data["ETAG"];

            bool onlineAccess = false;

            if (etag.value == "ONLINE_ACCESS")
            {
                onlineAccess = true;
            }

            var client = ClientManager.GetClient(clientId);

            TdfEncoder encoder = new TdfEncoder();

            if (onlineAccess == false)
            {
                TdfList nlst = new TdfList("NLST", TdfBaseType.TDF_TYPE_STRUCT, new ArrayList
                {
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2011-11-02T11:2Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1234632478),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "303566"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "DR:224766400"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "ONLINE_ACCESS"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 1),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2011-11-02T11:2Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1294632417),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "303566"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "303566"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "PROJECT10_CODE_CONSUMED_LE1"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 1),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2013-02-22T14:40Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 5674749135),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "306678"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "OFB-EAST:50401"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "BF3:PREMIUM_ACCESS"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 5),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2014-05-29T6:15Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1005150961807),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "305060"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "DR:235665900"),
                        new TdfInteger("STAT", 2),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "ONLINE_ACCESS"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 1),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2013-02-22T14:40Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1002134961807),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "305061"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "DR:235663400"),
                        new TdfInteger("STAT", 2),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "ONLINE_ACCESS"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 1),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2012-06-04T21:13Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1771457489),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "306678"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "OFB-EAST:50400"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "BF3:PREMIUM_ACCESS"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 5),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },

                    // DLC 1 - Back 2 Karkand
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2012-06-04T21:13Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1771457490),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "302777"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "OFB-EAST:50400"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "BF3:PC:B2K_PURCHASE"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 5),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },

                    // DLC 2
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2012-06-04T21:13Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1771457491),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "302776"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "OFB-EAST:48215"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "BF3:PC:XPACK2_PURCHASE"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 5),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },

                    // DLC 3
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2014-02-07T20:15Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1004743136441),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "302778"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "OFB-EAST:51080"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "BF3:PC:XPACK3_PURCHASE"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 5),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },

                    // DLC 4
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2012-11-26T9:4Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1000808118611),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "303129"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "OFB-EAST:55171"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "BF3:PC:XPACK4_PURCHASE"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 5),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },

                    // DLC 5
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2013-03-07T2:21Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1002246118611),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "306409"),
                        new TdfInteger("PRCA", 2),
                        new TdfString("PRID", "OFB-EAST:109546437"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "BF3:PC:XPACK5_PURCHASE"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 5),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    },

                    // COOP shortcut
                    new List<Tdf>
                    {
                        new TdfString("DEVI", ""),
                        new TdfString("GDAY", "2012-04-17T15:57Z"),
                        new TdfString("GNAM", "BF3PC"),
                        new TdfInteger("ID", 1684196754),
                        new TdfInteger("ISCO", 0),
                        new TdfInteger("PID", 0),
                        new TdfString("PJID", "306215"),
                        new TdfInteger("PRCA", 1),
                        new TdfString("PRID", "OFB-EAST:48642"),
                        new TdfInteger("STAT", 1),
                        new TdfInteger("STRC", 0),
                        new TdfString("TAG", "BF3:SHORTCUT:COOP"),
                        new TdfString("TDAY", ""),
                        new TdfInteger("TYPE", 5),
                        new TdfInteger("UCNT", 0),
                        new TdfInteger("VER", 0)
                    }
                });

                encoder.WriteTdf(new List<Tdf>
                {
                    nlst
                });
            }

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.AUTHENTICATION,
                commandId = 0x1D,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
