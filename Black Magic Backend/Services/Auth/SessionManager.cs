using System.Net.Sockets;
using Black_Magic_Backend.Models;

public class SessionManager {
    private readonly Dictionary<TcpClient, ClientSession> _connectedClients;

    public SessionManager(Dictionary<TcpClient, ClientSession> connectedClients) {
        _connectedClients = connectedClients;
    }

    public void RegisterClient(TcpClient client, Character character) {
        _connectedClients[client] = new ClientSession { Character = character };
    }

    public List<PlayerInfo> GetAllConnectedPlayers() {
        return _connectedClients.Values
            .Select(s => new PlayerInfo {
                Id = s.Character.Id.ToString(),
                Name = s.Character.Name,
                PositionX = s.Character.Position.X,
                PositionY = s.Character.Position.Y,
                PositionZ = s.Character.Position.Z
            })
            .ToList();
    }
}
