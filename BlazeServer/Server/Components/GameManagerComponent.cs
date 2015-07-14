using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class GameManagerComponent
    {
        public static string GetNotificationName(ushort notificationId)
        {
            string result = "";

            switch (notificationId)
            {
                case 0xA:
                    result = "NotifyMatchmakingFailed";
                    break;
                case 0xC:
                    result = "NotifyMatchmakingAsyncStatus";
                    break;
                case 0xF:
                    result = "NotifyGameCreated";
                    break;
                case 0x10:
                    result = "NotifyGameRemoved";
                    break;
                case 0x14:
                    result = "NotifyGameSetup";
                    break;
                case 0x15:
                    result = "NotifyPlayerJoining";
                    break;
                case 0x16:
                    result = "NotifyJoiningPlayerInitiateConnections";
                    break;
                case 0x17:
                    result = "NotifyPlayerJoiningQueue";
                    break;
                case 0x18:
                    result = "NotifyPlayerPromotedFromQueue";
                    break;
                case 0x19:
                    result = "NotifyPlayerClaimingReservation";
                    break;
                case 0x1E:
                    result = "NotifyPlayerJoinCompleted";
                    break;
                case 0x28:
                    result = "NotifyPlayerRemoved";
                    break;
                case 0x3C:
                    result = "NotifyHostMigrationFinished";
                    break;
                case 0x46:
                    result = "NotifyHostMigrationStart";
                    break;
                case 0x47:
                    result = "NotifyPlatformHostInitialized";
                    break;
                case 0x50:
                    result = "NotifyGameAttribChange";
                    break;
                case 0x5A:
                    result = "NotifyPlayerAttribChange";
                    break;
                case 0x5F:
                    result = "NotifyPlayerCustomDataChange";
                    break;
                case 0x64:
                    result = "NotifyGameStateChange";
                    break;
                case 0x6E:
                    result = "NotifyGameSettingsChange";
                    break;
                case 0x6F:
                    result = "NotifyGameCapacityChange";
                    break;
                case 0x70:
                    result = "NotifyGameReset";
                    break;
                case 0x71:
                    result = "NotifyGameReportingIdChange";
                    break;
                case 0x73:
                    result = "NotifyGameSessionUpdated";
                    break;
                case 0x74:
                    result = "NotifyGamePlayerStateChange";
                    break;
                case 0x75:
                    result = "NotifyGamePlayerTeamChange";
                    break;
                case 0x76:
                    result = "NotifyGameTeamIdChange";
                    break;
                case 0x77:
                    result = "NotifyProcessQueue";
                    break;
                case 0x78:
                    result = "NotifyPresenceModeChanged";
                    break;
                case 0x79:
                    result = "NotifyGamePlayerQueuePositionChange";
                    break;
                case 0xC9:
                    result = "NotifyGameListUpdate";
                    break;
                case 0xCA:
                    result = "NotifyAdminListChange";
                    break;
                case 0xDC:
                    result = "NotifyCreateDynamicDedicatedServerGame";
                    break;
                case 0xE6:
                    result = "NotifyGameNameChange";
                    break;
                default:
                    break;
            }

            return result;
        }

        public static string GetCommandName(ushort commandId)
        {
            string result = "";

            switch (commandId)
            {
                case 0x1:
                    result = "createGame";
                    break;
                case 0x2:
                    result = "destroyGame";
                    break;
                case 0x3:
                    result = "advanceGameState";
                    break;
                case 0x4:
                    result = "setGameSettings";
                    break;
                case 0x5:
                    result = "setPlayerCapacity";
                    break;
                case 0x6:
                    result = "setPresenceMode";
                    break;
                case 0x7:
                    result = "setGameAttributes";
                    break;
                case 0x8:
                    result = "setPlayerAttributes";
                    break;
                case 0x9:
                    result = "joinGame";
                    break;
                case 0xB:
                    result = "removePlayer";
                    break;
                case 0xD:
                    result = "startMatchmaking";
                    break;
                case 0xE:
                    result = "cancelMatchmaking";
                    break;
                case 0xF:
                    result = "finalizeGameCreation";
                    break;
                case 0x11:
                    result = "listGames";
                    break;
                case 0x12:
                    result = "setPlayerCustomData";
                    break;
                case 0x13:
                    result = "replayGame";
                    break;
                case 0x14:
                    result = "returnDedicatedServerToPool";
                    break;
                case 0x15:
                    result = "joinGameByGroup";
                    break;
                case 0x16:
                    result = "leaveGameByGroup";
                    break;
                case 0x17:
                    result = "migrateGame";
                    break;
                case 0x18:
                    result = "updateGameHostMigrationStatus";
                    break;
                case 0x19:
                    result = "resetDedicatedServer";
                    break;
                case 0x1A:
                    result = "updateGameSession";
                    break;
                case 0x1B:
                    result = "banPlayer";
                    break;
                case 0x1D:
                    result = "updateMeshConnection";
                    break;
                case 0x1F:
                    result = "removePlayerFromBannedList";
                    break;
                case 0x20:
                    result = "clearBannedList";
                    break;
                case 0x21:
                    result = "getBannedList";
                    break;
                case 0x26:
                    result = "addQueuedPlayerToGame";
                    break;
                case 0x27:
                    result = "updateGameName";
                    break;
                case 0x28:
                    result = "ejectHost";
                    break;
                case 0x64:
                    result = "getGameListSnapshot";
                    break;
                case 0x65:
                    result = "getGameListSubscription";
                    break;
                case 0x66:
                    result = "destroyGameList";
                    break;
                case 0x67:
                    result = "getFullGameData";
                    break;
                case 0x68:
                    result = "getMatchmakingConfig";
                    break;
                case 0x69:
                    result = "getGameDataFromId";
                    break;
                case 0x6A:
                    result = "addAdminPlayer";
                    break;
                case 0x6B:
                    result = "removeAdminPlayer";
                    break;
                case 0x6C:
                    result = "setPlayerTeam";
                    break;
                case 0x6D:
                    result = "changeGameTeamId";
                    break;
                case 0x6E:
                    result = "migrateAdminPlayer";
                    break;
                case 0x6F:
                    result = "getUserSetGameListSubscription";
                    break;
                case 0x70:
                    result = "swapPlayersTeam";
                    break;
                case 0x96:
                    result = "registerDynamicDedicatedServerCreator";
                    break;
                case 0x97:
                    result = "unregisterDynamicDedicatedServerCreator";
                    break;
                default:
                    break;
            }

            return result;
        }

        public static void HandleComponent(ulong clientId, Packet packet, SslStream stream)
        {
            switch (packet.commandId)
            {
                case 0x1:
                    CreateGameCommand.Handle(clientId, packet);
                    break;

                //case 0x2:
                //    Log.Warn("*destroyGame(game)");
                //    break;

                case 0x3:
                    AdvanceGameStateCommand.Handle(packet, stream);
                    break;

                case 0x5:
                    SetPlayerCapacityCommand.Handle(clientId, packet, stream);
                    break;

                case 0x7:
                    SetGameAttributesCommand.Handle(clientId, packet, stream);
                    break;

                case 0x9:
                    JoinGameCommand.Handle(clientId, packet, stream);
                    break;

                //case 0xB:
                //    //Log.Warn("*GameManager->HandleRemovePlayerCommand*");
                //    //HandleRemovePlayerCommand(clientId, packet, stream);
                //    break;

                case 0xF:
                    FinalizeGameCreationCommand.Handle(packet, stream);
                    break;

                case 0x1D:
                    UpdateMeshConnectionCommand.Handle(clientId, packet, stream);
                    break;

                default:
                    Utilities.LogUnhandledRequest(packet);
                    break;
            }
        }
    }
}
