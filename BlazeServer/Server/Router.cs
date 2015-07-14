using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;

namespace BlazeServer
{
    public class Router
    {
        public static void HandleRequest(ulong clientId, Packet packet, SslStream stream)
        {
            Utilities.LogRequest(packet);

            switch (packet.componentId)
            {
                case Component.REDIRECTOR:
                    RedirectorComponent.HandleComponent(clientId, packet, stream);
                    break;

                case Component.UTIL:
                    UtilComponent.HandleComponent(clientId, packet, stream);
                    break;

                case Component.AUTHENTICATION:
                    AuthenticationComponent.HandleComponent(clientId, packet, stream);
                    break;

                case Component.USERSESSIONS:
                    UserSessionsComponent.HandleComponent(clientId, packet, stream);
                    break;

                case Component.GAMEMANAGER:
                    GameManagerComponent.HandleComponent(clientId, packet, stream);
                    break;
                
                case Component.RSP:
                    RSPComponent.HandleComponent(clientId, packet, stream);
                    break;

                case Component.GAMEREPORTING:
                    GameReportingComponent.HandleComponent(clientId, packet, stream);
                    break;

                case Component.CLUBS:
                    ClubsComponent.HandleComponent(clientId, packet, stream);
                    break;

                case Component.STATS:
                    StatsComponent.HandleComponent(clientId, packet, stream);
                    break;

                default:
                    Utilities.LogUnhandledRequest(packet);
                    //SendError(packet, stream);
                    break;
            }
        }
    }
}