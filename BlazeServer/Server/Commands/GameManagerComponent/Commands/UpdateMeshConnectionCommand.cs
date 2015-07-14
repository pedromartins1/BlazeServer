using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class UpdateMeshConnectionCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfList targ = (TdfList)data["TARG"];
            List<Tdf> targData = (List<Tdf>)targ.list[0];
            TdfInteger stat = (TdfInteger)targData[2];

            TdfEncoder encoder = new TdfEncoder();
            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x1D,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);

            var client = ClientManager.GetClient(clientId);

            switch ((PlayerState)stat.value)
            {
                case PlayerState.DISCONNECTED:
                    // TODO: GameManager.RemovePlayer?
                    Log.Warn("*updateMeshConnection -> RemovePlayer*");
                    break;

                case PlayerState.CONNECTED:
                    GamePlayerStateChangeNotification.Notify(clientId, stream);
                    PlayerJoinCompletedNotification.Notify(client.gameId, client.persona.id, stream);
                    break;

                default:
                    Log.Warn("Unknown PlayerState in updateMeshCommand: " + stat.value);
                    break;
            }
        }
    }
}
