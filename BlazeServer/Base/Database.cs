using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Web.Script.Serialization;
using System.Linq;

using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace BlazeServer
{
    public class Database
    {
        public struct NetworkInfo
        {
            public ulong ip;
            public ushort port;
        }

        public struct Game
        {
            public ulong id;
            public ulong clientId;

            public string name;

            public Dictionary<object, object> attributes;
            public ArrayList capacity;

            public string level;
            public string gametype;

            public ushort maxPlayers;
            public byte notResetable; // bool
            public ushort queueCapacity;

            public PresenceMode presenceMode;
            public GameState state;
            public GameNetworkTopology networkTopology;
            public VoipTopology voipTopology;

            public NetworkInfo internalNetworkInfo;
            public NetworkInfo externalNetworkInfo;
        }

        public static string connectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", Configuration.DatabaseConfiguration.Host, Configuration.DatabaseConfiguration.Port, Configuration.DatabaseConfiguration.Database, Configuration.DatabaseConfiguration.Username, Configuration.DatabaseConfiguration.Password);

        public static void Initialize()
        {
            //Log.Info("Initializing database manager.");

            // truncate games table
            DeleteAllGames();
        }

        public static void DeleteAllGames()
        {
            Log.Debug("Deleting all games.");

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "TRUNCATE games";

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
        }

        public static ulong CreateGame(Game game)
        {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            ulong gameId = 0;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO games(clientid, name, attributes, capacity, level, gametype, max_players, not_resetable, queue_capacity, presence_mode, state, network_topology, voip_topology, internal_ip, internal_port, external_ip, external_port) VALUES(@clientid, @name, @attributes, @capacity, @level, @gametype, @max_players, @not_resetable, @queue_capacity, @presence_mode, @state, @network_topology, @voip_topology, @internal_ip, @internal_port, @external_ip, @external_port); SELECT LAST_INSERT_ID();";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@clientid", game.clientId);
                cmd.Parameters.AddWithValue("@name", game.name);
                cmd.Parameters.AddWithValue("@attributes", JsonConvert.SerializeObject(game.attributes));
                cmd.Parameters.AddWithValue("@capacity", JsonConvert.SerializeObject(game.capacity));
                cmd.Parameters.AddWithValue("@level", game.level);
                cmd.Parameters.AddWithValue("@gametype", game.gametype);
                cmd.Parameters.AddWithValue("@max_players", game.maxPlayers);
                cmd.Parameters.AddWithValue("@not_resetable", game.notResetable);
                cmd.Parameters.AddWithValue("@queue_capacity", game.queueCapacity);
                cmd.Parameters.AddWithValue("@presence_mode", game.presenceMode);
                cmd.Parameters.AddWithValue("@state", game.state);
                cmd.Parameters.AddWithValue("@network_topology", game.networkTopology);
                cmd.Parameters.AddWithValue("@voip_topology", game.voipTopology);

                cmd.Parameters.AddWithValue("@internal_ip", game.internalNetworkInfo.ip);
                cmd.Parameters.AddWithValue("@internal_port", game.internalNetworkInfo.port);

                cmd.Parameters.AddWithValue("@external_ip", game.externalNetworkInfo.ip);
                cmd.Parameters.AddWithValue("@external_port", game.externalNetworkInfo.port);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    gameId = reader.GetUInt64(0);
                }
            }
            catch (MySqlException ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }

                if (reader != null)
                {
                    reader.Dispose();
                }
            }

            ClientManager.UpdateClientGameID(game.clientId, gameId);

            Log.Info(string.Format("Created game {0} with name '{1}'.", gameId, game.name));

            return gameId;
        }

        public static void DeleteGame(ulong gameId)
        {
            MySqlConnection conn = null;

            Log.Info(string.Format("Deleting game {0}.", gameId));

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("DELETE FROM games WHERE id='{0}'", gameId);

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
        }

        public static void UpdateGameCapacity(ulong gameId, ArrayList capacity)
        {
            Log.Debug(string.Format("Updating game capacity for game {0}.", gameId));

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("UPDATE games SET capacity='{0}' WHERE id='{1}'", JsonConvert.SerializeObject(capacity), gameId);

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
        }

        public static void UpdateGamePresence(ulong gameId, PresenceMode presenceMode)
        {
            Log.Debug(string.Format("Updating game presence mode to {0} for game {1}.", presenceMode, gameId));

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("UPDATE games SET presence='{0}' WHERE id='{1}'", (ushort)presenceMode, gameId);

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
        }

        public static void UpdateGameState(ulong gameId, GameState state)
        {
            Log.Debug(string.Format("Updating game state to {0} for game {1}.", state, gameId));

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("UPDATE games SET state='{0}' WHERE id='{1}'", (ushort)state, gameId);

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
        }

        public static void UpdateGameMaxPlayers(ulong gameId, ushort maxPlayers)
        {
            Log.Info(string.Format("Updating max players to {0} for game {1}.", maxPlayers, gameId));

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("UPDATE games SET max_players='{0}' WHERE id='{1}'", maxPlayers, gameId);

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
        }

        public static bool GameExists(ulong gameId)
        {
            var exists = false;

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = string.Format("SELECT * FROM games WHERE id='{0}'", gameId);
                cmd.Connection = conn;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        exists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }

                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            return exists;
        }

        public static Dictionary<object, object> GetGameAttributes(ulong gameId)
        {
            var attributes = new Dictionary<object, object>();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = string.Format("SELECT * FROM games WHERE id='{0}'", gameId);
                cmd.Connection = conn;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    attributes = JsonConvert.DeserializeObject<Dictionary<object, object>>(reader.GetString("attributes"));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }

                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            return attributes;
        }

        public static void UpdateGameAttributes(ulong gameId, Dictionary<object, object> attributes)
        {
            Log.Debug(string.Format("Updating attributes for game {1}.", gameId));

            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("UPDATE games SET attributes='{0}' WHERE id='{1}'", JsonConvert.SerializeObject(attributes), gameId);

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
        }
        
        public static Game GetGameByID(ulong gameId)
        {
            var game = new Game();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = string.Format("SELECT * FROM games WHERE id='{0}'", gameId);
                cmd.Connection = conn;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    game.id = reader.GetUInt32("id");
                    game.clientId = reader.GetUInt32("clientid");
                    game.name = reader.GetString("name");
                    game.attributes = JsonConvert.DeserializeObject<Dictionary<object, object>>(reader.GetString("attributes"));
                    game.capacity = new ArrayList(JsonConvert.DeserializeObject<List<ulong>>(reader.GetString("capacity")).ToArray());
                    game.level = reader.GetString("level");
                    game.gametype = reader.GetString("gametype");
                    game.maxPlayers = reader.GetUInt16("max_players");
                    game.notResetable = reader.GetByte("not_resetable");
                    game.queueCapacity = reader.GetUInt16("queue_capacity");
                    game.presenceMode = (PresenceMode)reader.GetUInt16("presence_mode");
                    game.state = (GameState)reader.GetUInt16("state");
                    game.networkTopology = (GameNetworkTopology)reader.GetUInt16("network_topology");
                    game.voipTopology = (VoipTopology)reader.GetUInt16("voip_topology");

                    var internalNetworkInfo = new NetworkInfo();
                    var externalNetworkInfo = new NetworkInfo();

                    internalNetworkInfo.ip = (ulong)reader.GetUInt32("internal_ip");
                    internalNetworkInfo.port = reader.GetUInt16("internal_port");

                    externalNetworkInfo.ip = (ulong)reader.GetUInt32("external_ip");
                    externalNetworkInfo.port = reader.GetUInt16("external_port");

                    game.internalNetworkInfo = internalNetworkInfo;
                    game.externalNetworkInfo = externalNetworkInfo;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }

                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            return game;
        }

        public struct User
        {
            public ulong id;
            public string mail;
            public string password;
            public ulong personaId;
        }

        public struct Persona
        {
            public ulong id;
            public string name;
            public ulong lastLogin;
        }

        public static User GetUser(ulong personaId)
        {
            var user = new User();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = string.Format("SELECT * FROM users WHERE personaid='{0}'", personaId);
                cmd.Connection = conn;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.id = reader.GetUInt64("id");
                    user.mail = reader.GetString("mail");
                    user.password = reader.GetString("password");
                    user.personaId = reader.GetUInt64("personaid");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }

                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            return user;
        }

        public static User GetUser(string mail, string password)
        {
            var user = new User();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = string.Format("SELECT * FROM users WHERE mail='{0}' AND password='{1}'", mail, password);
                cmd.Connection = conn;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.id = reader.GetUInt64("id");
                    user.mail = reader.GetString("mail");
                    user.password = reader.GetString("password");
                    user.personaId = reader.GetUInt64("personaid");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }

                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            return user;
        }

        public static Persona GetPersona(ulong personaId)
        {
            var persona = new Persona();

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = string.Format("SELECT * FROM personas WHERE id='{0}'", personaId);
                cmd.Connection = conn;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    persona.id = reader.GetUInt64("id");
                    persona.name = reader.GetString("name");
                    persona.lastLogin = reader.GetUInt64("last_login");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }

                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            return persona;
        }
    }
}
