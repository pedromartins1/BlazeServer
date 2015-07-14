using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class PreAuthCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            // decode payload
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            // read client type
            TdfStruct cdat = (TdfStruct)data["CDAT"];
            TdfInteger type = (TdfInteger)cdat.data.Find(tdf => tdf.label == "TYPE");
            TdfString svcn = (TdfString)cdat.data.Find(tdf => tdf.label == "SVCN");

            TdfStruct cinf = (TdfStruct)data["CINF"];
            TdfInteger loc = (TdfInteger)cinf.data.Find(tdf => tdf.label == "LOC");

            // set client type
            ClientManager.UpdateClientType(clientId, (ClientType)type.value);
            ClientManager.UpdateClientLocalization(clientId, loc.value);
            ClientManager.UpdateClientService(clientId, svcn.value);

            var client = ClientManager.GetClient(clientId);

            TdfList cids = new TdfList("CIDS", TdfBaseType.TDF_TYPE_INTEGER, new ArrayList
            {
                //1, 25, 4, 27, 28, 6, 7, 9, 10, 11, 30720, 30721, 30722, 30723, 20, 30725, 30726, 2000
            });
            cids.list.AddRange((new ulong[] { 1, 25, 4, 27, 28, 6, 7, 9, 10, 11, 30720, 30721, 30722, 30723, 20, 30725, 30726, 2000 }).ToArray());

            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfInteger("ANON", 0),
                new TdfString("ASRC", "300294"),
                cids,
                new TdfString("CNGN", ""),
                new TdfStruct("CONF", new List<Tdf>
                {
                    new TdfMap("CONF", TdfBaseType.TDF_TYPE_STRING, TdfBaseType.TDF_TYPE_STRING, new Dictionary<object, object>
                    {
                        { "connIdleTimeout", "90s" },
                        { "defaultRequestTimeout", "80s" },
                        { "pingPeriod", "20s" },
                        { "voipHeadsetUpdateRate", "1000" },
                        { "xlspConnectionIdleTimeout", "300" }
                    })
                }),
                new TdfString("INST", client.service),
                new TdfInteger("MINR", 0),
                new TdfString("NASP", "cem_ea_id"), // TODO: check if present in decoded data
                new TdfString("PILD", ""),
                new TdfString("PLAT", "pc"), // TODO: fetch from decoded data
                new TdfString("PTAG", ""),
                new TdfStruct("QOSS", new List<Tdf>
                {
                    // bandwidth ping site info
                    new TdfStruct("BWPS", new List<Tdf>
                    {
                        new TdfString("PSA", "127.0.0.1"), // ping site address
                        new TdfInteger("PSP", 17502), // ping site port
                        new TdfString("SNA", "ams") // ping site name
                    }), 
                    new TdfInteger("LNP", 10), // number of latency probes
                    new TdfMap("LTPS", TdfBaseType.TDF_TYPE_STRING, TdfBaseType.TDF_TYPE_STRUCT, new Dictionary<object, object>
                    {
                        { "ams", new List<Tdf>
                            {
                                new TdfString("PSA", "127.0.0.1"), // ping site address
                                new TdfInteger("PSP", 17502), // ping site port
                                new TdfString("SNA", "ams") // ping site name
                            }
                        }
                    }),
                    new TdfInteger("SVID", 1161889797) // service ID
                }),
                new TdfString("RSRC", "300294"),
                new TdfString("SVER", "Blaze 3.15.08.0 (CL# 1060080)")
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.UTIL,
                commandId = 0x7,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
