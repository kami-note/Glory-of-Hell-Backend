using Newtonsoft.Json;

public struct PlayerInfo
{
    [JsonProperty("id")]
    public string Id;
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("positionX")]
    public float PositionX;
    [JsonProperty("positionY")]
    public float PositionY;
    [JsonProperty("positionZ")]
    public float PositionZ;
}