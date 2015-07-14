using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class ClientManager
    {
        public struct Client
        {
            public SslStream stream; // used by playerjoining notification

            public Database.NetworkInfo internalNetworkInfo;
            public Database.NetworkInfo externalNetworkInfo;

            public ClientType type;
            public ulong localization;
            public string service;

            public ulong gameId;

            public string authToken;

            public ulong viewId;
            public ArrayList entityIds;

            public ulong LastTouched;

            public Database.User user;
            public Database.Persona persona;
        }

        public static Dictionary<ulong, Client> clients { get; set; }

        public static void Initialize()
        {
            //Log.Info("Initializing client manager.");

            clients = new Dictionary<ulong, Client>();
        }

        public static void AddClient(ulong clientId, EndPoint ep, SslStream stream)
        {
            IPEndPoint ipep = (IPEndPoint)ep;

            Client client = new Client();

            client.stream = stream;

            Database.NetworkInfo externalNetworkInfo;
            externalNetworkInfo.ip = (ulong)Utilities.IPToLong(ipep.Address.ToString());
            externalNetworkInfo.port = (ushort)ipep.Port;

            client.externalNetworkInfo = externalNetworkInfo;

            client.LastTouched = 0;

            clients.Add(clientId, client);
        }

        public static void CleanClients()
        {
            lock (clients)
            {
                var canBeSafelyDeleted = (from client in clients
                                          where (Utilities.GetUnixTime() - client.Value.LastTouched) > 80000
                                          select client.Key).ToList();

                foreach (var client in canBeSafelyDeleted)
                {
                    /* if (clients[client].type == ClientType.CLIENT_TYPE_DEDICATED_SERVER)
                    {
                        var gameId = clients[client].gameId;

                        Log.Info(string.Format("Deleting game in CleanClients because {0}", (Utilities.GetUnixTime() - clients[client].LastTouched)));

                        Database.DeleteGame(gameId);
                    }

                    clients.Remove(client); */
                }

                Log.Debug(string.Format("Deleted {0} clients.", canBeSafelyDeleted.Count));
                canBeSafelyDeleted = null;
            }
        }

        public static Client GetClient(ulong clientId)
        {
            Client client;

            if (clients.TryGetValue(clientId, out client))
            {

            }

            return client;
        }

        public static void UpdateViewID(ulong clientId, ulong viewId)
        {
            Log.Debug(string.Format("Updating client {0} viewId: {1}.", clientId, viewId));

            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.viewId = viewId;

                clients[clientId] = client;
            }
        }

        public static void UpdateEntityIDs(ulong clientId, ArrayList entityIds)
        {
            Log.Debug(string.Format("Updating client {0} entityIds.", clientId));

            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.entityIds = entityIds;

                clients[clientId] = client;
            }
        }

        public static void UpdateClientAuthToken(ulong clientId, string authToken)
        {
            Log.Debug(string.Format("Updating client {0} authToken: {1}.", clientId, authToken));

            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.authToken = authToken;

                clients[clientId] = client;
            }
        }

        public static void UpdateClientGameID(ulong clientId, ulong gameId)
        {
            Log.Debug(string.Format("Updating client {0} gameId: {1}.", clientId, gameId));

            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.gameId = gameId;

                clients[clientId] = client;
            }
        }

        public static void SetClientUser(ulong clientId, Database.User user)
        {
            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.user = user;

                clients[clientId] = client;
            }
        }

        public static void SetClientPersona(ulong clientId, Database.Persona persona)
        {
            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.persona = persona;

                clients[clientId] = client;
            }
        }

        public static void UpdateClientService(ulong clientId, string service)
        {
            Log.Debug(string.Format("Updating client {0} service: {1}.", clientId, service));

            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.service = service;

                clients[clientId] = client;
            }
        }

        public static void UpdateClientLocalization(ulong clientId, ulong localization)
        {
            Log.Debug(string.Format("Updating client {0} localization.", clientId));

            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.localization = localization;

                clients[clientId] = client;
            }
        }

        public static void UpdateClientType(ulong clientId, ClientType clientType)
        {
            Log.Debug(string.Format("Updating client {0} type: {1}.", clientId, clientType));

            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.type = clientType;

                clients[clientId] = client;
            }
        }

        public static void UpdateClientLastSeen(ulong clientId, ulong time)
        {
            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.LastTouched = time;

                clients[clientId] = client;
            }
        }

        public static void UpdateClientInternalNetworkData(ulong clientId, ulong internalIP, ushort internalPort)
        {
            Log.Debug(string.Format("Updating client {0} internal network data.", clientId));

            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.internalNetworkInfo.ip = internalIP;
                client.internalNetworkInfo.port = internalPort;

                // external port should be same as internal port (25200 or so)
                client.externalNetworkInfo.port = internalPort;

                // TODO: rewrite this properly; export should be set in UpdateNetworkInfo or so,
                // not in TCPServer code

                clients[clientId] = client;
            }
        }

        public static void UpdateClientExternalNetworkData(ulong clientId, ulong externalIP, ushort externalPort)
        {
            Log.Debug(string.Format("Updating client {0} external network data.", clientId));

            Client client;

            if (clients.TryGetValue(clientId, out client))
            {
                client.externalNetworkInfo.ip = externalIP;
                //client.export = externalPort;

                clients[clientId] = client;
            }
        }
    }
}
