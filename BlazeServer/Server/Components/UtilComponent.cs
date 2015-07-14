using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class UtilComponent
    {
        public static string GetCommandName(ushort commandId)
        {
            string result = "";

            switch (commandId)
            {
                case 0x1:
                    result = "fetchClientConfig";
                    break;
                case 0x2:
                    result = "ping";
                    break;
                case 0x3:
                    result = "setClientData";
                    break;
                case 0x4:
                    result = "localizeStrings";
                    break;
                case 0x5:
                    result = "getTelemetryServer";
                    break;
                case 0x6:
                    result = "getTickerServer";
                    break;
                case 0x7:
                    result = "preAuth";
                    break;
                case 0x8:
                    result = "postAuth";
                    break;
                case 0xA:
                    result = "userSettingsLoad";
                    break;
                case 0xB:
                    result = "userSettingsSave";
                    break;
                case 0xC:
                    result = "userSettingsLoadAll";
                    break;
                case 0xE:
                    result = "deleteUserSettings";
                    break;
                case 0x14:
                    result = "filterForProfanity";
                    break;
                case 0x15:
                    result = "fetchQosConfig";
                    break;
                case 0x16:
                    result = "setClientMetrics";
                    break;
                case 0x17:
                    result = "setConnectionState";
                    break;
                case 0x18:
                    result = "getPssConfig";
                    break;
                case 0x19:
                    result = "getUserOptions";
                    break;
                case 0x1A:
                    result = "setUserOptions";
                    break;
                case 0x1B:
                    result = "suspendUserPing";
                    break;
                default:
                    break;
            }

            return result;
        }

        public static void HandleComponent(ulong clientId, Packet packet, SslStream stream)
        {
            switch ((short)packet.commandId)
            {
                case 0x1:
                    FetchClientConfigCommand.Handle(packet, stream);
                    break;

                case 0x2:
                    PingCommand.Handle(clientId, packet, stream);
                    break;

                case 0x5:
                    GetTelemetryServerCommand.Handle(clientId, packet, stream);
                    break;

                case 0x6:
                    // getTickerServer
                    break;

                case 0x7:
                    PreAuthCommand.Handle(clientId, packet, stream);
                    break;

                case 0x8:
                    PostAuthCommand.Handle(clientId, packet, stream);
                    break;

                case 0xA:
                    // userSettingsLoad
                    break;

                case 0xB:
                    UserSettingsSaveCommand.Handle(clientId, packet, stream);
                    break;

                case 0xC:
                    UserSettingsLoadAllCommand.Handle(clientId, packet, stream);
                    break;

                default:
                    Utilities.LogUnhandledRequest(packet);
                    break;
            }
        }
    }
}
