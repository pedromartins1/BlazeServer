using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class PostAuthCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            var client = ClientManager.GetClient(clientId);

            Log.Info(string.Format("Performing post-auth for user {0}.", client.user.id));

            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfStruct("PSS", new List<Tdf>
                {
                    // address
                    new TdfString("ADRS", "127.0.0.1"),
                    new TdfBlob("CSIG", new byte[] { }),                   
                    //new TdfList("OIDS", 0), // offer IDs, probably a list of Vector3's
                    new TdfString("PJID", "123071"), // project ID
                    new TdfInteger("PORT", 8443), // port
                    new TdfInteger("RPRT", 9), // initial report types
                    new TdfInteger("TIID", 0) // title ID
                }),
                new TdfStruct("TELE", new List<Tdf>
                {
                    new TdfString("ADRS", "127.0.0.1"),
                    new TdfInteger("ANON", 0),

                    // telemetryDisable
                    new TdfString("DISA", "AD,AF,AG,AI,AL,AM,AN,AO,AQ,AR,AS,AW,AX,AZ,BA,BB,BD,BF,BH,BI,BJ,BM,BN,BO,BR,BS,BT,BV,BW,BY,BZ,CC,CD,CF,CG,CI,CK,CL,CM,CN,CO,CR,CU,CV,CX,DJ,DM,DO,DZ,EC,EG,EH,ER,ET,FJ,FK,FM,FO,GA,GD,GE,GF,GG,GH,GI,GL,GM,GN,GP,GQ,GS,GT,GU,GW,GY,HM,HN,HT,ID,IL,IM,IN,IO,IQ,IR,IS,JE,JM,JO,KE,KG,KH,KI,KM,KN,KP,KR,KW,KY,KZ,LA,LB,LC,LI,LK,LR,LS,LY,MA,MC,MD,ME,MG,MH,ML,MM,MN,MO,MP,MQ,MR,MS,MU,MV,MW,MY,MZ,NA,NC,NE,NF,NG,NI,NP,NR,NU,OM,PA,PE,PF,PG,PH,PK,PM,PN,PS,PW,PY,QA,RE,RS,RW,SA,SB,SC,SD,SG,SH,SJ,SL,SM,SN,SO,SR,ST,SV,SY,SZ,TC,TD,TF,TG,TH,TJ,TK,TL,TM,TN,TO,TT,TV,TZ,UA,UG,UM,UY,UZ,VA,VC,VE,VG,VN,VU,WF,WS,YE,YT,ZM,ZW,ZZ"),
                    new TdfString("FILT", ""),
                    new TdfInteger("LOC", client.localization),

                    // telemetryNoToggleOk
                    new TdfString("NOOK", "US,CA,MX"),
                    new TdfInteger("PORT", 9988),

                    new TdfInteger("SDLY", 15000),
                    new TdfString("SESS", "telemetry_session"),
                    new TdfString("SKEY", "telemetry_key"),
                    new TdfInteger("SPCT", 75),
                    new TdfString("STIM", "Default")
                }),
                new TdfStruct("TICK", new List<Tdf>
                {
                    new TdfString("ADRS", "127.0.0.1"),
                    new TdfInteger("PORT", 8999),
                    new TdfString("SKEY", string.Format("{0},127.0.0.1:8999,battlefield-3-pc,10,50,50,50,50,0,12", client.persona.id))
                }),
                new TdfStruct("UROP", new List<Tdf>
                {
                    new TdfInteger("TMOP", (ulong)TelemetryOpt.TELEMETRY_OPT_IN),
                    new TdfInteger("UID", client.persona.id)
                })
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.UTIL,
                commandId = 0x8,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
