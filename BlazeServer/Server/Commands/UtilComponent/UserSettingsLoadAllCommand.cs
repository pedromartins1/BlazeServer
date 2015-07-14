using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class UserSettingsLoadAllCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            TdfMap smap = new TdfMap("SMAP", TdfBaseType.TDF_TYPE_STRING, TdfBaseType.TDF_TYPE_STRING, new Dictionary<object, object>
            {
                { "cust", "" } // TODO: fetch from userSettingsSave - 0x000B
            });

            encoder.WriteTdf(new List<Tdf>
            {

            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.UTIL,
                commandId = 0xC,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
