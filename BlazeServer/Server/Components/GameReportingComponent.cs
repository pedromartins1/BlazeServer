using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class GameReportingComponent
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
                    result = "submitGameReport";
                    break;
                case 0x64:
                    result = "submitTrustedMidGameReport";
                    break;
                case 0x65:
                    result = "submitTrustedEndGameReport";
                    break;
                case 0x2:
                    result = "submitOfflineGameReport";
                    break;
                case 0x3:
                    result = "submitGameEvents";
                    break;
                case 0x4:
                    result = "getGameReportQuery";
                    break;
                case 0x5:
                    result = "getGameReportQueriesList";
                    break;
                case 0x6:
                    result = "getGameReports";
                    break;
                case 0x7:
                    result = "getGameReportView";
                    break;
                case 0x8:
                    result = "getGameReportViewInfo";
                    break;
                case 0x9:
                    result = "getGameReportViewInfoList";
                    break;
                case 0xA:
                    result = "getGameReportTypes";
                    break;
                case 0xB:
                    result = "updateMetric";
                    break;
                case 0xC:
                    result = "getGameReportColumnInfo";
                    break;
                case 0xD:
                    result = "getGameReportColumnValues";
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
                default:
                    Utilities.LogUnhandledRequest(packet);
                    break;
            }
        }
    }
}
