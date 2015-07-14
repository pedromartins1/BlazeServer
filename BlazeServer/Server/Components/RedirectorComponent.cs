using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class RedirectorComponent
    {
        public static string GetCommandName(ushort commandId)
        {
            string result = "";

            switch (commandId)
            {
                case 0x1:
                    result = "getServerInstance";
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
                    GetServerInstanceCommand.Handle(packet, stream);
                    break;

                default:
                    Utilities.LogUnhandledRequest(packet);
                    break;
            }
        }
    }
}
