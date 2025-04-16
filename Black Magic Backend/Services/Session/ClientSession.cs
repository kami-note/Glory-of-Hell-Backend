using System.Net.Sockets;
using Black_Magic_Backend.Models;

public class ClientSession {
    public Character Character { get; set; } = new Character();
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
    
    public TcpClient TcpClient { get; set; } = new TcpClient();
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    public bool IsAuthenticated { get; set; } = false;

    public string IpAddress => TcpClient?.Client?.RemoteEndPoint?.ToString() ?? "Unknown";
}
