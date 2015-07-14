using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class SetPlayerCapacityCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfInteger gid = (TdfInteger)data["GID"];
            TdfList pcap = (TdfList)data["PCAP"];

            Database.UpdateGameCapacity(gid.value, pcap.list);

            TdfEncoder encoder = new TdfEncoder();

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x5,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);

            GameCapacityChangeNotification.Notify(gid.value, pcap.list, stream);
        }
    }
}
