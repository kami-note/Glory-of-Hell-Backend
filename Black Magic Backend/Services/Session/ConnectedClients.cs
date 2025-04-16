using System.Net.Sockets;
using Black_Magic_Backend.Models;

public static class ConnectedClients
{
    private static readonly Dictionary<TcpClient, Character> _clients = new();

    public static void Add(TcpClient client, Character character)
    {
        _clients[client] = character;
    }

    public static void Remove(TcpClient client)
    {
        _clients.Remove(client);
    }

    public static Character? GetCharacter(TcpClient client)
    {
        return _clients.TryGetValue(client, out var character) ? character : null;
    }

    public static List<Character> GetAll()
    {
        return _clients.Values.ToList();
    }
}
