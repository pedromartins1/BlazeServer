using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class GamePlayerStateChangeNotification
    {
        public static void Notify(ulong clientId, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            var client = ClientManager.GetClient(clientId);
            var game = Database.GetGameByID(client.gameId);

            encoder.WriteTdf(new List<Tdf>
            {
                new TdfInteger("GID", client.gameId),
                new TdfInteger("PID", client.persona.id),
                new TdfInteger("STAT", 4) // GameState.POST_GAME? // TODO: get enum for this, probably GameState or so
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x74,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
