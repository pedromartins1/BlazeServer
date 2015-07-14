using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class CreateGameCommand
    {
        public static void Handle(ulong clientId, Packet packet)
        {
            var client = ClientManager.GetClient(clientId);

            Dictionary<string, Tdf> data = Utilities.DecodePayload(packet.payload);

            TdfMap attr = (TdfMap)data["ATTR"];
            TdfString gnam = (TdfString)data["GNAM"];
            TdfInteger gset = (TdfInteger)data["GSET"];
            TdfList pcap = (TdfList)data["PCAP"];
            TdfInteger igno = (TdfInteger)data["IGNO"];
            TdfInteger pmax = (TdfInteger)data["PMAX"];
            TdfInteger nres = (TdfInteger)data["NRES"];

            // network topology
            TdfInteger ntop = (TdfInteger)data["NTOP"];
            TdfInteger voip = (TdfInteger)data["VOIP"];

            TdfInteger pres = (TdfInteger)data["PRES"]; // TdfMin
            TdfInteger qcap = (TdfInteger)data["QCAP"];
            //TdfString uuid = (TdfString)data["UUID"];

            TdfList hnet = (TdfList)data["HNET"];

            TdfStruct exip = (TdfStruct)hnet.list[0];
            TdfInteger exipIP = (TdfInteger)exip.data.Find(tdf => tdf.label == "IP");
            TdfInteger exipPort = (TdfInteger)exip.data.Find(tdf => tdf.label == "PORT");

            TdfStruct inip = (TdfStruct)hnet.list[1];
            TdfInteger inipIP = (TdfInteger)inip.data.Find(tdf => tdf.label == "IP");
            TdfInteger inipPort = (TdfInteger)inip.data.Find(tdf => tdf.label == "PORT");

            // TODO: don't get gameId as result but get by clientId after creating the game
            /* ulong gameId = GameManager.CreateGame(

                (int)gset.value,
                (int)igno.value,
                (int)nres.value,
                (int)ntop.value,
                "714b05dc-93bc-49ac-961c-cb38b574f30a"
            ); */

            var level = attr.map["level"].ToString();
            var gametype = attr.map["levellocation"].ToString();

            var game = new Database.Game();
            game.clientId = clientId;
            game.name = gnam.value;
            game.attributes = attr.map;
            game.capacity = pcap.list;
            game.level = attr.map["level"].ToString();
            game.gametype = attr.map["levellocation"].ToString();
            game.maxPlayers = (ushort)pmax.value;
            game.notResetable = (byte)nres.value;
            game.queueCapacity = (ushort)qcap.value;
            game.presenceMode = (PresenceMode)pres.value;
            game.state = GameState.INITIALIZING;
            game.networkTopology = (GameNetworkTopology)ntop.value;
            game.voipTopology = (VoipTopology)voip.value;

            Database.NetworkInfo internalNetworkInfo = new Database.NetworkInfo();
            internalNetworkInfo.ip = inipIP.value;
            internalNetworkInfo.port = (ushort)inipPort.value;

            Database.NetworkInfo externalNetworkInfo = new Database.NetworkInfo();
            externalNetworkInfo.ip = exipIP.value;
            externalNetworkInfo.port = (ushort)exipPort.value;

            game.internalNetworkInfo = internalNetworkInfo;
            game.externalNetworkInfo = externalNetworkInfo;

            ulong gameId = Database.CreateGame(game);

            TdfEncoder encoder = new TdfEncoder();

            encoder.WriteTdf(new List<Tdf>
            {
                // this one is tdfmin
                new TdfInteger("GID", (ulong)gameId)
            });

            byte[] payload = encoder.Encode();

            Utilities.SendPacket(new Packet
            {
                componentId = Component.GAMEMANAGER,
                commandId = 0x1,
                errorCode = 0,
                msgType = MessageType.REPLY,
                msgNum = packet.msgNum,

                payload = payload,
                payloadSize = payload.Length
            }, client.stream);

            GameStateChangeNotification.Notify(gameId, game.state, client.stream);
            GameSetupNotification.Notify(clientId, gameId, client.stream);
        }
    }
}
