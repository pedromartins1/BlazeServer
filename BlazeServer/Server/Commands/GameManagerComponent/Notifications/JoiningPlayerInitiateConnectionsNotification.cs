using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class JoiningPlayerInitiateConnectionsNotification
    {
        public static void Notify(ulong clientId, SslStream stream)
        {
            TdfEncoder encoder = new TdfEncoder();

            var client = ClientManager.GetClient(clientId);
            var game = Database.GetGameByID(client.gameId);

            encoder.WriteTdf(new List<Tdf>
            {
               new TdfStruct("GAME", new List<Tdf>
                {
                    new TdfList("ADMN", TdfBaseType.TDF_TYPE_INTEGER, new ArrayList { client.persona.id }),
                    new TdfMap("ATTR", TdfBaseType.TDF_TYPE_STRING, TdfBaseType.TDF_TYPE_STRING, game.attributes),
                    new TdfList("CAP", TdfBaseType.TDF_TYPE_INTEGER, game.capacity),
                    new TdfInteger("GID", (ulong)client.gameId),
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
                    new TdfInteger("IGNO", 0),
                    new TdfInteger("MCAP", game.maxPlayers),
                    new TdfStruct("NQQS", new List<Tdf>
                    {
                        new TdfInteger("DBPS", 0),
                        new TdfInteger("NATT", 0), // TODO: get from DB
                        new TdfInteger("UBPS", 0)
                    }),
                    new TdfInteger("NRES", (ulong)game.notResetable),
                    new TdfInteger("NTOP", (ulong)game.networkTopology),
                    new TdfString("PGID", ""),
                    new TdfBlob("PGSR", new byte[] { }),
                    new TdfStruct("PHST", new List<Tdf>
                    {
                        new TdfInteger("HPID", client.persona.id),
                        new TdfInteger("HSLT", 1)
                    }),
                    new TdfInteger("PRES", (ulong)game.presenceMode),
                    new TdfString("PSAS", "ams"),
                    new TdfInteger("QCAP", (ulong)game.queueCapacity),
                    new TdfInteger("SEED", 2291),
                    new TdfInteger("TCAP", 0),
                    new TdfStruct("THST", new List<Tdf>
                    {
                        new TdfInteger("HPID", (ulong)client.gameId),
                        new TdfInteger("HSLT", 0)
                    }),
                    new TdfString("UUID", ""),
                    new TdfInteger("VOIP", (ulong)game.voipTopology),
                    new TdfString("VSTR", "67")
                }),
                new TdfList("PROS", TdfBaseType.TDF_TYPE_STRUCT, new ArrayList
                {
                    new List<Tdf>
                    {
                        new TdfBlob("BLOB", new byte[] { }),
                        new TdfInteger("EXID", 0),
                        new TdfInteger("GID", client.gameId),
                        new TdfInteger("LOC", client.localization),
                        new TdfString("NAME", client.persona.name),
                        new TdfMap("PATT", TdfBaseType.TDF_TYPE_STRING, TdfBaseType.TDF_TYPE_STRING, new Dictionary<object, object>
                        {
                            { "Premium", "False" }
                        }),
                        new TdfInteger("PID", client.persona.id),
                        new TdfUnion("PNET", NetworkAddressMember.MEMBER_IPPAIRADDRESS, new List<Tdf>
                        {
                            new TdfStruct("VALU", new List<Tdf>
                            {
                                new TdfStruct("EXIP", new List<Tdf>
                                {
                                    new TdfInteger("IP", client.externalNetworkInfo.ip),
                                    new TdfInteger("PORT", client.externalNetworkInfo.port)
                                }),
                                new TdfStruct("INIP", new List<Tdf>
                                {
                                    new TdfInteger("IP", client.internalNetworkInfo.ip),
                                    new TdfInteger("PORT", client.internalNetworkInfo.port)
                                })
                            })
                        }),
                        new TdfInteger("SID", 1), // TODO: check what the fuck is this...
                        new TdfInteger("SLOT", 0),
                        new TdfInteger("STAT", 2), // TODO: get enum for this, it's probably ACTIVE
                        new TdfInteger("TIDX", 65535),
                        new TdfInteger("TIME", 0), // TODO: time goes here?
                        new TdfVector3("UGID", 0, 0, 0),
                        new TdfInteger("UID", client.persona.id)
                    }
                }),
                new TdfInteger("REAS", 0),
                new TdfStruct("VALU", new List<Tdf>
                {
                    new TdfInteger("DCTX", 3)
                })
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x16,
                errorCode = 0,
                msgType = MessageType.NOTIFICATION,
                msgNum = 0,

                payload = payload,
                payloadSize = payload.Length
            }, stream);
        }
    }
}
