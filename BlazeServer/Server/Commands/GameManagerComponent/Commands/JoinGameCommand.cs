using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class JoinGameCommand
    {
        public static void Handle(ulong clientId, Packet packet, SslStream stream)
        {
            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfInteger gid = (TdfInteger)data["GID"];
            TdfUnion pnet = (TdfUnion)data["PNET"];
            TdfStruct valu = (TdfStruct)pnet.data.Find(tdf => tdf.label == "VALU");

            TdfStruct exip = (TdfStruct)valu.data.Find(tdf => tdf.label == "EXIP");
            TdfInteger exipIP = (TdfInteger)exip.data.Find(tdf => tdf.label == "IP");
            TdfInteger exipPort = (TdfInteger)exip.data.Find(tdf => tdf.label == "PORT");

            TdfStruct inip = (TdfStruct)valu.data.Find(tdf => tdf.label == "INIP");
            TdfInteger inipIP = (TdfInteger)inip.data.Find(tdf => tdf.label == "IP");
            TdfInteger inipPort = (TdfInteger)inip.data.Find(tdf => tdf.label == "PORT");

            var client = ClientManager.GetClient(clientId);

            if (Database.GameExists(gid.value))
            {
                // update stuff
                ClientManager.UpdateClientInternalNetworkData(clientId, inipIP.value, (ushort)inipPort.value);
                ClientManager.UpdateClientExternalNetworkData(clientId, exipIP.value, (ushort)inipPort.value);
                ClientManager.UpdateClientGameID(clientId, (ulong)gid.value);

                Log.Info(string.Format("User {0} is joining game {1}.", client.user.id, gid.value));

                TdfEncoder encoder = new TdfEncoder();

                encoder.WriteTdf(new List<Tdf>
                {
                    new TdfInteger("GID", (ulong)gid.value),
                    new TdfInteger("JGS", 0)
                });

                byte[] payload = encoder.Encode();

                Utilities.SendPacket(new Packet
                {
                    componentId = Component.GAMEMANAGER,
                    commandId = 0x9,
                    errorCode = 0,
                    msgType = MessageType.REPLY,
                    msgNum = packet.msgNum,

                    payload = payload,
                    payloadSize = payload.Length
                }, stream);

                var game = Database.GetGameByID(gid.value);
                var gameClient = ClientManager.GetClient(game.clientId);

                // TODO: check if only userupdated needed
                UserAddedNotification.Notify(clientId, stream, true);
                UserUpdatedNotification.Notify(client.persona.id, stream);

                JoiningPlayerInitiateConnectionsNotification.Notify(clientId, stream);
                UserSessionExtendedDataUpdateNotification.Notify(clientId, stream, true);

                PlayerJoiningNotification.Notify(clientId, gameClient.stream);
                PlayerClaimingReservationNotification.Notify(clientId, gameClient.stream);
                UserSessionExtendedDataUpdateNotification.Notify(clientId, gameClient.stream, true);
            }
            else
            {
                Log.Warn(string.Format("User {0} wanted to a game that doesn't exist ({1}).", client.user.id, gid.value));

                /*
                 * not sure if we should set the error code to GAMEMANAGER_ERR_NO_DEDICATED_SERVER_FOUND (0x12D0004)
                 * or GAMEMANAGER_ERR_INVALID_GAME_ID (0x20004)
                 * */

                Utilities.SendPacket(new Packet
                {
                    componentId = Component.GAMEMANAGER,
                    commandId = 0x9,
                    errorCode = 0x20004, // GAMEMANAGER_ERR_INVALID_GAME_ID
                    msgType = MessageType.ERROR_REPLY,
                    msgNum = packet.msgNum,

                    payload = null,
                    payloadSize = 0
                }, stream);
            }
        }
    }
}
