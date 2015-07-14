using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class SetGameAttributesCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfMap attr = (TdfMap)data["ATTR"];
            TdfInteger gid = (TdfInteger)data["GID"];

            Log.Info(string.Format("Setting game attributes for game {0}.", gid.value));

            var game = Database.GetGameByID(gid.value);
            var attributes = game.attributes;

            foreach (var key in attr.map.Keys)
            {
                attributes[key] = attr.map[key];
            }

            Database.UpdateGameAttributes(gid.value, attributes);

            TdfEncoder encoder = new TdfEncoder();

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x7,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);

            GameAttribChangeNotification.Notify(gid.value, attr.map, stream);
        }
    }
}
