using System.Net;
using Black_Magic_Backend.Models;

public static class ConnectedClients
{
    private static readonly Dictionary<IPEndPoint, Character> _clients = new();

    public static void Add(IPEndPoint endpoint, Character character)
    {
        _clients[endpoint] = character;
    }

    public static void Remove(IPEndPoint endpoint)
    {
        _clients.Remove(endpoint);
    }

    public static Character? GetCharacter(IPEndPoint endpoint)
    {
        return _clients.TryGetValue(endpoint, out var character) ? character : null;
    }

    public static List<Character> GetAll()
    {
        return _clients.Values.ToList();
    }
}
