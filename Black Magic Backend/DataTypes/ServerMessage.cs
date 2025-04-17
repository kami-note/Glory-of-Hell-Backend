using Newtonsoft.Json;
public struct ServerMessage
{
    [JsonProperty("action")]
    public string Action => "ServerMessage";
    [JsonProperty("message")]
    public string Message { get; set; }
}