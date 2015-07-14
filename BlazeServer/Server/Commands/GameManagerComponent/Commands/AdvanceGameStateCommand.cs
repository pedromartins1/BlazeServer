using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class AdvanceGameStateCommand
    {
        public static void Handle(Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfInteger gid = (TdfInteger)data["GID"];
            TdfInteger gsta = (TdfInteger)data["GSTA"];

            // update game state
            Database.UpdateGameState(gid.value, (GameState)gsta.value);

            Log.Info(string.Format("Advancing game state to {0} for game {1}.", (GameState)gsta.value, gid.value));

            TdfEncoder encoder = new TdfEncoder();

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = packet.componentId,
                commandId = 0x3,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, stream);

            GameStateChangeNotification.Notify(gid.value, (GameState)gsta.value, stream);
        }
    }
}
