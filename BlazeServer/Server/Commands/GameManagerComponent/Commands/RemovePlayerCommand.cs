using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class RemovePlayerCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            var client = ClientManager.GetClient(clientId);

            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfInteger gameId = (TdfInteger)data["GID"];
            TdfInteger personaId = (TdfInteger)data["PID"];
            TdfInteger reason = (TdfInteger)data["REAS"];

            // TODO: GameManager.RemovePlayer

            TdfEncoder encoder = new TdfEncoder();

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = packet.componentId,
                commandId = 0xB,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);

            // send a notification that a player has been removed
            PlayerRemovedNotification.Notify(gameId.value, personaId.value, (PlayerRemovedReason)reason.value, stream);
        }
    }
}
