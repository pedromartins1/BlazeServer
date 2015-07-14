using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class UpdateNetworkInfoCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfUnion addr = (TdfUnion)data["ADDR"];
            TdfStruct valu = (TdfStruct)addr.data.Find(tdf => tdf.label == "VALU");

            TdfStruct inip = (TdfStruct)valu.data.Find(tdf => tdf.label == "INIP");
            TdfInteger ip = (TdfInteger)inip.data.Find(tdf => tdf.label == "IP");
            TdfInteger port = (TdfInteger)inip.data.Find(tdf => tdf.label == "PORT");

            Log.Debug(string.Format("Updating internal network info for client {0}.", clientId));
            ClientManager.UpdateClientInternalNetworkData(clientId, ip.value, (ushort)port.value);

            TdfEncoder encoder = new TdfEncoder();

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.USERSESSIONS,
                commandId = 0x14,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
