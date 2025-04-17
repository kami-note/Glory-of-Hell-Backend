
using Newtonsoft.Json;

public struct InitialSyncData
{
    [JsonProperty("action")]
    public string Action => "InitialSync";
    [JsonProperty("localPlayer")]
    public PlayerInfo LocalPlayer;
    [JsonProperty("otherPlayers")]
    public List<PlayerInfo> OtherPlayers;
}