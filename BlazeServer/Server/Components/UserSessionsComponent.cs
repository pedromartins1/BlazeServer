using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class UserSessionsComponent
    {
        public static string GetNotificationName(ushort notificationId)
        {
            string result = "";

            switch (notificationId)
            {
                case 0x1:
                    result = "UserSessionExtendedDataUpdate";
                    break;
                case 0x2:
                    result = "UserAdded";
                    break;
                case 0x3:
                    result = "UserRemoved";
                    break;
                case 0x4:
                    result = "UserSessionDisconnected";
                    break;
                case 0x5:
                    result = "UserUpdated";
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
                case 0x3:
                    result = "fetchExtendedData";
                    break;
                case 0x5:
                    result = "updateExtendedDataAttribute";
                    break;
                case 0x8:
                    result = "updateHardwareFlags";
                    break;
                case 0xC:
                    result = "lookupUser";
                    break;
                case 0xD:
                    result = "lookupUsers";
                    break;
                case 0xE:
                    result = "lookupUsersByPrefix";
                    break;
                case 0x14:
                    result = "updateNetworkInfo";
                    break;
                case 0x17:
                    result = "lookupUserGeoIPData";
                    break;
                case 0x18:
                    result = "overrideUserGeoIPData";
                    break;
                case 0x19:
                    result = "updateUserSessionClientData";
                    break;
                case 0x1A:
                    result = "setUserInfoAttribute";
                    break;
                case 0x1B:
                    result = "resetUserGeoIPData";
                    break;
                case 0x20:
                    result = "lookupUserSessionId";
                    break;
                case 0x21:
                    result = "fetchLastLocaleUsedAndAuthError";
                    break;
                case 0x22:
                    result = "fetchUserFirstLastAuthTime";
                    break;
                case 0x23:
                    result = "resumeSession";
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
                case 0x14:
                    UpdateNetworkInfoCommand.Handle(clientId, packet, stream);
                    UserSessionExtendedDataUpdateNotification.Notify(clientId, stream);
                    break;

                default:
                    Utilities.LogUnhandledRequest(packet);
                    break;
            }
        }
    }
}
