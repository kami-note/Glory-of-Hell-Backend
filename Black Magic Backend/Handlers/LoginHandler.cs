using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Black_Magic_Backend.Handlers
{
    public class LoginHandler : IMessageHandler
    {
        private readonly AuthSystem _authSystem;
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<IPEndPoint, ClientSession> _connectedClients;

        public LoginHandler(AuthSystem authSystem, ApplicationDbContext dbContext, Dictionary<IPEndPoint, ClientSession> connectedClients)
        {
            _authSystem = authSystem;
            _dbContext = dbContext;
            _connectedClients = connectedClients;
        }

        public async Task HandleAsync(UdpClient client, JObject json, IPEndPoint remoteEndPoint)
        {
            bool success = _authSystem.AuthenticateUser(json.ToString());

            if (success)
            {
                var username = json["username"]?.ToString();
                var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    var character = _dbContext.Characters.FirstOrDefault(c => c.User.Id == user.Id);
                    if (character != null)
                    {
                        _connectedClients[remoteEndPoint] = new ClientSession { Character = character };
                        var characterNames = _connectedClients.Values.Select(session => session.Character.Name).ToList();
                        PrettyConsole.LogInfo($"The list of connected clients' characters: {string.Join(", ", characterNames)}");
                        PrettyConsole.LogWarning($"User {username} logged in successfully.");
                    }
                }
            }

            string response = success ? "Login successful!" : "Login error.";
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await client.SendAsync(responseBytes, responseBytes.Length, remoteEndPoint);
        }
    }
}
