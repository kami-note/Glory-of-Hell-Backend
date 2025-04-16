using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Text;

namespace Black_Magic_Backend.Handlers {
    public class LoginHandler : IMessageHandler {
        private readonly AuthSystem _authSystem;
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<TcpClient, ClientSession> _connectedClients;

        public LoginHandler(AuthSystem authSystem, ApplicationDbContext dbContext, Dictionary<TcpClient, ClientSession> connectedClients) {
            _authSystem = authSystem;
            _dbContext = dbContext;
            _connectedClients = connectedClients;
        }

        public async Task HandleAsync(TcpClient client, JObject json) {
            var stream = client.GetStream();
            bool success = _authSystem.AuthenticateUser(json.ToString());

            if (success) {
                var username = json["username"]?.ToString();
                var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

                if (user != null) {
                    var character = _dbContext.Characters.FirstOrDefault(c => c.User.Id == user.Id);
                    if (character != null) {
                        _connectedClients[client] = new ClientSession { Character = character };
                        var characterNames = _connectedClients.Values.Select(s => s.Character.Name).ToList();
                        PrettyConsole.LogInfo($"The list of connected clients' characters: {string.Join(", ", characterNames)}");
                        PrettyConsole.LogWarning($"User {username} logged in successfully.");
                    }
                }
            }

            string response = success ? "Login successful!" : "Login error.";
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}
