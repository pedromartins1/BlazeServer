using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class AuthenticationComponent
    {
        public static string GetCommandName(ushort commandId)
        {
            string result = "";

            if (commandId > 300)
            {
                if (commandId == 500)
                {
                    result = "checkSinglePlayerLogin";
                }   
            }
            else if (commandId == 300)
            {
                result = "deviceLoginGuest";
            }
            else
            {
                switch (commandId)
                {
                    case 0xA:
                        result = "createAccount";
                        break;
                    case 0x14:
                        result = "updateAccount";
                        break;
                    case 0x1E:
                        result = "getAccount";
                        break;
                    case 0x28:
                        result = "login";
                        break;
                    case 0x29:
                        result = "acceptTos";
                        break;
                    case 0xF1:
                        result = "acceptLegalDocs";
                        break;
                    case 0x2A:
                        result = "getTosInfo";
                        break;
                    case 0xF2:
                        result = "getLegalDocsInfo";
                        break;
                    case 0x2C:
                        result = "consumecode";
                        break;
                    case 0x2D:
                        result = "passwordForgot";
                        break;
                    case 0x2E:
                        result = "getTermsAndConditionsContent";
                        break;
                    case 0xF6:
                        result = "getTermsOfServiceContent";
                        break;
                    case 0x2F:
                        result = "getPrivacyPolicyContent";
                        break;
                    case 0x32:
                        result = "silentLogin";
                        break;
                    case 0x33:
                        result = "checkAgeReq";
                        break;
                    case 0x98:
                        result = "originLogin";
                        break;
                    case 0x3C:
                        result = "expressLogin";
                        break;
                    case 0x46:
                        result = "logout";
                        break;
                    case 0x50:
                        result = "createPersona";
                        break;
                    case 0x5A:
                        result = "getPersona";
                        break;
                    case 0x64:
                        result = "listPersonas";
                        break;
                    case 0x6E:
                        result = "loginPersona";
                        break;
                    case 0x78:
                        result = "logoutPersona";
                        break;
                    case 0x8C:
                        result = "deletePersona";
                        break;
                    case 0x8D:
                        result = "disablePersona";
                        break;
                    case 0x8F:
                        result = "listDeviceAccounts";
                        break;
                    case 0x96:
                        result = "xboxCreateAccount";
                        break;
                    case 0xA0:
                        result = "xboxAssociateAccount";
                        break;
                    case 0xAA:
                        result = "xboxLogin";
                        break;
                    case 0xB4:
                        result = "ps3CreateAccount";
                        break;
                    case 0xBE:
                        result = "ps3AssociateAccount";
                        break;
                    case 0xC8:
                        result = "ps3Login";
                        break;
                    case 0xD2:
                        result = "validateSessionKey";
                        break;
                    case 0x1C:
                        result = "updateParentalEmail";
                        break;
                    case 0x1D:
                        result = "listUserEntitlements2";
                        break;
                    case 0x30:
                        result = "listPersonaEntitlements2";
                        break;
                    case 0x27:
                        result = "grantEntitlement2";
                        break;
                    case 0x1F:
                        result = "grantEntitlement";
                        break;
                    case 0x20:
                        result = "listEntitlements";
                        break;
                    case 0x21:
                        result = "hasEntitlement";
                        break;
                    case 0x22:
                        result = "getUseCount";
                        break;
                    case 0x23:
                        result = "decrementUseCount";
                        break;
                    case 0x24:
                        result = "getAuthToken";
                        break;
                    case 0x25:
                        result = "getHandoffToken";
                        break;
                    case 0x26:
                        result = "getPasswordRules";
                        break;
                    case 0x2B:
                        result = "modifyEntitlement2";
                        break;
                    case 0x34:
                        result = "getOptIn";
                        break;
                    case 0x35:
                        result = "enableOptIn";
                        break;
                    case 0x36:
                        result = "disableOptIn";
                        break;
                    case 0xE6:
                        result = "createWalUserSession";
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public static void HandleComponent(ulong clientId, Packet packet, SslStream stream)
        {
            var client = ClientManager.GetClient(clientId);

            switch (packet.commandId)
            {
                case 0x28:
                    LoginCommand.Handle(clientId, packet, stream);
                    break;

                case 0x32:
                    SilentLoginCommand.Handle(clientId, packet, stream);

                    UserAddedNotification.Notify(clientId, stream);
                    UserUpdatedNotification.Notify(client.persona.id, stream);
                    break;

                case 0x6E:
                    LoginPersonaCommand.Handle(clientId, packet, stream);

                    UserAddedNotification.Notify(clientId, stream);
                    UserUpdatedNotification.Notify(client.persona.id, stream);
                    break;

                case 0x1D:
                    ListUserEntitlements2Command.Handle(clientId, packet, stream);
                    break;

                default:
                    Utilities.LogUnhandledRequest(packet);
                    break;
            }
        }
    }
}
