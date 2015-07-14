namespace BlazeServer
{
    public enum Component : ushort
    {
        AUTHENTICATION = 0x1,
        GAMEMANAGER = 0x4,
        REDIRECTOR = 0x5,
        STATS = 0x7,
        UTIL = 0x9,
        CLUBS = 0xB,
        GAMEREPORTING = 0x1C,
        RSP = 0x801,
        USERSESSIONS = 0x7802
    };

    public enum MessageType : ushort
    {
        MESSAGE = 0x0,
        REPLY = 0x1,
        NOTIFICATION = 0x2,
        ERROR_REPLY = 0x3
    };

    public struct Packet
    {
        public int headerSize;
        public Component componentId;
        public ushort commandId;
        public int errorCode;
        public MessageType msgType;
        public int msgNum;
        public int payloadSize;
        public byte[] payload;
    }

    public enum NetworkAddressMember : ushort
    {
        MEMBER_XBOXCLIENTADDRESS = 0x0,
        MEMBER_XBOXSERVERADDRESS = 0x1,
        MEMBER_IPPAIRADDRESS = 0x2,
        MEMBER_IPADDRESS = 0x3,
        MEMBER_HOSTNAMEADDRESS = 0x4,
        MEMBER_UNSET = 0x7F
    };

    public enum ExternalRefType : ushort
    {
        BLAZE_EXTERNAL_REF_TYPE_UNKNOWN = 0x0,
        BLAZE_EXTERNAL_REF_TYPE_XBOX = 0x1,
        BLAZE_EXTERNAL_REF_TYPE_PS3 = 0x2,
        BLAZE_EXTERNAL_REF_TYPE_WII = 0x3,
        BLAZE_EXTERNAL_REF_TYPE_MOBILE = 0x4,
        BLAZE_EXTERNAL_REF_TYPE_LEGACYPROFILEID = 0x5,
        BLAZE_EXTERNAL_REF_TYPE_TWITTER = 0x6,
        BLAZE_EXTERNAL_REF_TYPE_FACEBOOK = 0x7
    };

    public enum UpnpStatus : ushort
    {
        UPNP_UNKNOWN = 0x0,
        UPNP_FOUND = 0x1,
        UPNP_ENABLED = 0x2
    };

    public enum NatType : ushort
    {
        NAT_TYPE_OPEN = 0x0,
        NAT_TYPE_MODERATE = 0x1,
        NAT_TYPE_STRICT_SEQUENTIAL = 0x2,
        NAT_TYPE_STRICT = 0x3,
        NAT_TYPE_UNKNOWN = 0x4
    };

    public enum TelemetryOpt : ushort
    {
        TELEMETRY_OPT_OUT = 0x0,
        TELEMETRY_OPT_IN = 0x1
    };

    public enum GameEntryType : ushort
    {
        GAME_ENTRY_TYPE_DIRECT = 0x0,
        GAME_ENTRY_TYPE_MAKE_RESERVATION = 0x1,
        GAME_ENTRY_TYPE_CLAIM_RESERVATION = 0x2
    };

    public enum VoipTopology : ushort
    {
        VOIP_DISABLED = 0x0,
        VOIP_DEDICATED_SERVER = 0x1,
        VOIP_PEER_TO_PEER = 0x2
    };

    public enum GameNetworkTopology : ushort
    {
        CLIENT_SERVER_PEER_HOSTED = 0x0,
        CLIENT_SERVER_DEDICATED = 0x1,
        PEER_TO_PEER_FULL_MESH = 0x82,
        PEER_TO_PEER_PARTIAL_MESH = 0x83,
        PEER_TO_PEER_DIRTYCAST_FAILOVER = 0x84
    };

    public enum DatalessContext : ushort
    {
        CREATE_GAME_SETUP_CONTEXT = 0x0,
        JOIN_GAME_SETUP_CONTEXT = 0x1,
        INDIRECT_JOIN_GAME_FROM_QUEUE_SETUP_CONTEXT = 0x2,
        INDIRECT_JOIN_GAME_FROM_RESERVATION_CONTEXT = 0x3,
        HOST_INJECTION_SETUP_CONTEXT = 0x4
    };

    public enum ClientType : ushort
    {
        CLIENT_TYPE_GAMEPLAY_USER = 0x0,
        CLIENT_TYPE_HTTP_USER = 0x1,
        CLIENT_TYPE_DEDICATED_SERVER = 0x2,
        CLIENT_TYPE_TOOLS = 0x3,
        CLIENT_TYPE_INVALID = 0x4
    };

    public enum GameState : ushort
    {
        NEW_STATE = 0x0,
        INITIALIZING = 0x1,
        VIRTUAL = 0x2,
        PRE_GAME = 0x82,
        IN_GAME = 0x83,
        POST_GAME = 0x4,
        MIGRATING = 0x5,
        DESTRUCTING = 0x6,
        RESETABLE = 0x7,
        REPLAY_SETUP = 0x8
    };

    public enum PlayerState : ushort
    {
        DISCONNECTED = 0x0,
        CONNECTED = 0x2
    };

    public enum PresenceMode : ushort
    {
        PRESENCE_MODE_NONE = 0x0,
        PRESENCE_MODE_STANDARD = 0x1,
        PRESENCE_MODE_PRIVATE = 0x2
    };

    public enum PlayerRemovedReason : ushort
    {
        PLAYER_JOIN_TIMEOUT = 0x0,
        PLAYER_CONN_LOST = 0x1,
        BLAZESERVER_CONN_LOST = 0x2,
        MIGRATION_FAILED = 0x3,
        GAME_DESTROYED = 0x4,
        GAME_ENDED = 0x5,
        PLAYER_LEFT = 0x6,
        GROUP_LEFT = 0x7,
        PLAYER_KICKED = 0x8,
        PLAYER_KICKED_WITH_BAN = 0x9,
        PLAYER_JOIN_FROM_QUEUE_FAILED = 0xA,
        PLAYER_RESERVATION_TIMEOUT = 0xB,
        HOST_EJECTED = 0xC
    };

    public enum GameReportPlayerFinishedStatus : ushort
    {
        GAMEREPORT_FINISHED_STATUS_DEFAULT = 0x0,
        GAMEREPORT_FINISHED_STATUS_FINISHED = 0x1,
        GAMEREPORT_FINISHED_STATUS_DNF = 0x2
    };
}