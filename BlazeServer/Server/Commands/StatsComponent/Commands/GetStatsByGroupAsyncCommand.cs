using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class GetStatsByGroupAsyncCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);
            TdfInteger vid = (TdfInteger)data["VID"];
            TdfList eid = (TdfList)data["EID"];

            ClientManager.UpdateViewID(clientId, vid.value);
            ClientManager.UpdateEntityIDs(clientId, eid.list);

            Utilities.SendPacket(new Packet
            {
                componentId = Component.STATS,
                commandId = 0x10,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = null,
                payloadSize = 0
            }, stream);
        }
    }
}
