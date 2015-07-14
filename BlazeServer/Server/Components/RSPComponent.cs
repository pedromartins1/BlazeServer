using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class RSPComponent
    {
        public static string GetCommandName(ushort commandId)
        {
            string result = "";

            switch (commandId)
            {
                case 0xA:
                    result = "startPurchase";
                    break;
                case 0xB:
                    result = "updatePurchase";
                    break;
                case 0xC:
                    result = "finishPurchase";
                    break;
                case 0xF:
                    result = "listPurchases";
                    break;
                case 0x14:
                    result = "listServers";
                    break;
                case 0x15:
                    result = "getServerDetails";
                    break;
                case 0x17:
                    result = "restartServer";
                    break;
                case 0x18:
                    result = "updateServerBanner";
                    break;
                case 0x19:
                    result = "updateServerSettings";
                    break;
                case 0x1A:
                    result = "updateServerPreset";
                    break;
                case 0x1B:
                    result = "updateServerMapRotation";
                    break;
                case 0x1F:
                    result = "addServerAdmin";
                    break;
                case 0x20:
                    result = "removeServerAdmin";
                    break;
                case 0x21:
                    result = "addServerBan";
                    break;
                case 0x22:
                    result = "removeServerBan";
                    break;
                case 0x23:
                    result = "addServerVip";
                    break;
                case 0x24:
                    result = "removeServerVip";
                    break;
                case 0x32:
                    result = "getConfig";
                    break;
                case 0x33:
                    result = "getPingSites";
                    break;
                case 0x3C:
                    result = "getGameData";
                    break;
                case 0x3D:
                    result = "addGameBan";
                    break;
                case 0x46:
                    result = "createServer";
                    break;
                case 0x47:
                    result = "updateServer";
                    break;
                case 0x48:
                    result = "listAllServers";
                    break;
                case 0x50:
                    result = "startMatch";
                    break;
                case 0x51:
                    result = "abortMatch";
                    break;
                case 0x52:
                    result = "endMatch";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        public static void HandleComponent(ulong clientId, Packet packet, SslStream stream)
        {
            switch (packet.commandId)
            {
                //case 0x32:
                //    Log.Warn("RSPComponent.getConfig is unhandled");
                //    break;

                default:
                    Utilities.LogUnhandledRequest(packet);
                    break;
            }
        }
    }
}
