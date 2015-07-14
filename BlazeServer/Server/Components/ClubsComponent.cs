using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class ClubsComponent
    {
        public static string GetCommandName(ushort commandId)
        {
            string result;

            if (commandId <= 3000)
            {
                if (commandId == 3000)
                    return "acceptPetition";
                if (commandId > 2150)
                {
                    if (commandId > 2510)
                    {
                        if (commandId > 2700)
                        {
                            if (commandId == 2800)
                                return "sendPetition";
                            if (commandId == 2900)
                                return "getPetitions";
                        }
                        else
                        {
                            if (commandId == 2700)
                                return "getClubMembershipForUsers";
                            if (commandId == 2600)
                                return "getClubsComponentSettings";
                            if (commandId == 2650)
                                return "transferOwnership";
                        }
                    }
                    else
                    {
                        if (commandId == 2510)
                            return "setMetadata2";
                        if (commandId > 2400)
                        {
                            if (commandId == 2450)
                                return "setNewsItemHidden";
                            if (commandId == 2500)
                                return "setMetadata";
                        }
                        else
                        {
                            if (commandId == 2400)
                                return "getNews";
                            if (commandId == 2200)
                                return "updateClubSettings";
                            if (commandId == 2300)
                                return "postNews";
                        }
                    }
                }
                else
                {
                    if (commandId == 2150)
                        return "demoteToMember";
                    if (commandId > 1600)
                    {
                        if (commandId > 1900)
                        {
                            if (commandId == 2000)
                                return "getMembers";
                            if (commandId == 2100)
                                return "promoteToGM";
                        }
                        else
                        {
                            if (commandId == 1900)
                                return "declineInvitation";
                            if (commandId == 1700)
                                return "revokeInvitation";
                            if (commandId == 1800)
                                return "acceptInvitation";
                        }
                    }
                    else
                    {
                        if (commandId == 1600)
                            return "getInvitations";
                        if (commandId > 1310)
                        {
                            if (commandId == 1400)
                                return "removeMember";
                            if (commandId == 1500)
                                return "sendInvitation";
                        }
                        else
                        {
                            if (commandId == 1310)
                                return "findClubs2";
                            if (commandId == 1100)
                                return "createClub";
                            if (commandId == 1200)
                                return "getClubs";
                            if (commandId == 1300)
                                return "findClubs";
                        }
                    }
                }
                return "";
            }
            if (commandId <= 4100)
            {
                if (commandId == 4100)
                    return "setClubTickerMessagesSubscription";
                if (commandId > 3600)
                {
                    if (commandId > 3810)
                    {
                        if (commandId == 3900)
                            return "listRivals";
                        if (commandId == 4000)
                            return "getClubTickerMessages";
                    }
                    else
                    {
                        if (commandId == 3810)
                            return "findClubs2Async";
                        if (commandId == 3700)
                            return "updateMemberMetadata";
                        if (commandId == 3800)
                            return "findClubsAsync";
                    }
                }
                else
                {
                    if (commandId == 3600)
                        return "getClubAwards";
                    if (commandId > 3400)
                    {
                        if (commandId == 3410)
                            return "resetClubRecords";
                        if (commandId == 3500)
                            return "updateMemberOnlineStatus";
                    }
                    else
                    {
                        if (commandId == 3400)
                            return "getClubRecordbook";
                        if (commandId == 3100)
                            return "declinePetition";
                        if (commandId == 3200)
                            return "revokePetition";
                        if (commandId == 3300)
                            return "joinClub";
                    }
                }
                return "";
            }
            if (commandId <= 4700)
            {
                if (commandId == 4700)
                    return "banMember";
                if (commandId > 4400)
                {
                    if (commandId == 4500)
                        return "getClubBans";
                    if (commandId == 4600)
                        return "getUserBans";
                }
                else
                {
                    if (commandId == 4400)
                        return "getMembersAsync";
                    if (commandId == 4200)
                        return "changeClubStrings";
                    if (commandId == 4300)
                        return "countMessages";
                }
                return "";
            }
            if (commandId <= 5000)
            {
                if (commandId == 5000)
                    return "disbandClub";
                if (commandId == 4800)
                    return "unbanMember";
                if (commandId == 4900)
                    return "GetClubsComponentInfo";
                return "";
            }
            if (commandId == 5100)
            {
                result = "getNewsForClubs";
            }
            else
            {
                if (commandId != 5200)
                    result = "";
                result = "getPetitionsForClubs";
            }
            return result;
        }

        public static void HandleComponent(ulong clientId, Packet packet, SslStream stream)
        {
            switch (packet.commandId)
            {
                case 0xA8C:
                    GetClubMembershipForUsersCommand.Handle(packet, stream);
                    break;

                default:
                    Utilities.LogUnhandledRequest(packet);
                    break;
            }
        }
    }
}
