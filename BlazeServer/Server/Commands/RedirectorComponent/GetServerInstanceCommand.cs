using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class GetServerInstanceCommand
    {
        public static void Handle(Packet packet, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfUnion("ADDR", NetworkAddressMember.MEMBER_XBOXCLIENTADDRESS, new List<Tdf>
                {
                    new TdfStruct("VALU", new List<Tdf>
                    {
                        new TdfString("HOST", "373244-gosprapp357.ea.com"),
                        new TdfInteger("IP", 0),
                        new TdfInteger("PORT", 10041)
                    })
                }),
                new TdfInteger("SECU", 1),
                new TdfInteger("XDNS", 0)
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.REDIRECTOR,
                commandId = 0x1,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
