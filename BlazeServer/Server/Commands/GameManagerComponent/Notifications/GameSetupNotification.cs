using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class GameSetupNotification
    {
        public static void Notify(ulong clientId, ulong gameId, SslStream stream)
        {
            var client = ClientManager.GetClient(clientId);
            var game = Database.GetGameByID(gameId);

            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new TdfStruct("GAME", new List<Tdf>
            {
                new TdfList("ADMN", TdfBaseType.TDF_TYPE_INTEGER, new ArrayList
                {
                    client.persona.id
                }),
                new TdfMap("ATTR", TdfBaseType.TDF_TYPE_STRING, TdfBaseType.TDF_TYPE_STRING, game.attributes),
                new TdfList("CAP", TdfBaseType.TDF_TYPE_INTEGER, game.capacity),
                // CRIT
                new TdfInteger("GID", game.id),
                new TdfString("GNAM", game.name),
                new TdfInteger("GPVH", 666),
                //new TdfInteger("GSET", game.gset),
                new TdfInteger("GSID", 1),
                new TdfInteger("GSTA", (ulong)game.state),
                new TdfString("GTYP", "frostbite_multiplayer"),
                new TdfList("HNET", TdfBaseType.TDF_TYPE_STRUCT, new ArrayList
                {
                    new List<Tdf>
                    {
                        new TdfStruct("EXIP", new List<Tdf>
                        {
                            new TdfInteger("IP", game.externalNetworkInfo.ip),
                            new TdfInteger("PORT", game.externalNetworkInfo.port)
                        }),
                        new TdfStruct("INIP", new List<Tdf>
                        {
                            new TdfInteger("IP", game.internalNetworkInfo.ip),
                            new TdfInteger("PORT", game.internalNetworkInfo.port)
                        })
                    }
                }, true),
                new TdfInteger("HSES", 13666),
                /* new TdfInteger("IGNO", game.igno), */
                new TdfInteger("MCAP", game.maxPlayers),
                new TdfInteger("NRES", game.notResetable),
                new TdfInteger("NTOP", (ulong)game.networkTopology),
                new TdfString("PGID", "b6852db1-ba37-4b40-aea3-0bd16efba4f9"),
                new TdfBlob("PGSR", new byte[] { }),
                new TdfStruct("PHST", new List<Tdf>
                {
                    new TdfInteger("HPID", client.persona.id),
                    new TdfInteger("HSLT", 1)
                }),
                new TdfInteger("PRES", (ulong)game.presenceMode),
                new TdfString("PSAS", "ams"),
                new TdfInteger("QCAP", (ulong)game.queueCapacity),
                new TdfUnion("REAS", NetworkAddressMember.MEMBER_XBOXCLIENTADDRESS, new List<Tdf> { }),
                new TdfStruct("VALU", new List<Tdf>
                {
                    new TdfInteger("DCTX", 0)
                }),
                new TdfInteger("SEED", 2291),
                new TdfInteger("TCAP", 0),
                new TdfStruct("THST", new List<Tdf>
                {
                    new TdfInteger("HPID", client.gameId),
                    new TdfInteger("HSLT", 0)
                }),
                //new TdfString("UUID", game.uuid),
                new TdfInteger("VOIP", (ulong)game.voipTopology),
                new TdfString("VSTR", "67")
            }));

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x14,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
