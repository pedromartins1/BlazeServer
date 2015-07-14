using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class StatsComponent
    {
        public static string GetNotificationName(ushort notificationId)
        {
            string result = "";

            switch (notificationId)
            {
                case 0x32:
                    result = "GetStatsAsyncNotification";
                    break;
                case 0x33:
                    result = "GetLeaderboardTreeNotification";
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
                    result = "getStatDescs";
                    break;
                case 0x2:
                    result = "getStats";
                    break;
                case 0x3:
                    result = "getStatGroupList";
                    break;
                case 0x4:
                    result = "getStatGroup";
                    break;
                case 0x5:
                    result = "getStatsByGroup";
                    break;
                case 0x6:
                    result = "getDateRange";
                    break;
                case 0x7:
                    result = "getEntityCount";
                    break;
                case 0xA:
                    result = "getLeaderboardGroup";
                    break;
                case 0xB:
                    result = "getLeaderboardFolderGroup";
                    break;
                case 0xC:
                    result = "getLeaderboard";
                    break;
                case 0xD:
                    result = "getCenteredLeaderboard";
                    break;
                case 0xE:
                    result = "getFilteredLeaderboard";
                    break;
                case 0xF:
                    result = "getKeyScopesMap";
                    break;
                case 0x10:
                    result = "getStatsByGroupAsync";
                    break;
                case 0x11:
                    result = "getLeaderboardTreeAsync";
                    break;
                case 0x12:
                    result = "getLeaderboardEntityCount";
                    break;
                case 0x13:
                    result = "getStatCategoryList";
                    break;
                case 0x14:
                    result = "getPeriodIds";
                    break;
                case 0x15:
                    result = "getLeaderboardRaw";
                    break;
                case 0x16:
                    result = "getCenteredLeaderboardRaw";
                    break;
                case 0x17:
                    result = "getFilteredLeaderboardRaw";
                    break;
                case 0x18:
                    result = "changeKeyscopeValue";
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
                case 0x4:
                    Utilities.SendPacket(new Packet
                    {
                        componentId = packet.componentId,
                        commandId = packet.commandId,
                        errorCode = 0,
                        msgType = MessageType.ERROR_REPLY,
                        msgNum = packet.msgNum,

                        payload = null,
                        payloadSize = 0
                    }, stream);

                    //GetStatGroupCommand.Handle(clientId, packet, stream);
                    break;

                case 0x10:
                    Utilities.SendPacket(new Packet
                    {
                        componentId = packet.componentId,
                        commandId = packet.commandId,
                        errorCode = 0,
                        msgType = MessageType.ERROR_REPLY,
                        msgNum = packet.msgNum,

                        payload = null,
                        payloadSize = 0
                    }, stream);

                    //GetStatsByGroupAsyncCommand.Handle(clientId, packet, stream);
                    //GetStatsAsyncNotification.Notify(clientId, stream);
                    break;

                default:
                    Utilities.SendPacket(new Packet
                    {
                        componentId = packet.componentId,
                        commandId = packet.commandId,
                        errorCode = 0,
                        msgType = MessageType.ERROR_REPLY,
                        msgNum = packet.msgNum,

                        payload = null,
                        payloadSize = 0
                    }, stream);

                    Utilities.LogUnhandledRequest(packet);
                    break;
            }
        }
    }
}
