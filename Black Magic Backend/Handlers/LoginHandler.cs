using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Text;

namespace Black_Magic_Backend.Handlers
{
    public class LoginHandler : IMessageHandler
    {
        private readonly AuthSystem _authSystem;
        private readonly ApplicationDbContext _dbContext;
        private readonly SessionManager _sessionManager;

        public LoginHandler(AuthSystem authSystem, ApplicationDbContext dbContext, SessionManager sessionManager)
        {
            _authSystem = authSystem;
            _dbContext = dbContext;
            _sessionManager = sessionManager;
        }

        public async Task HandleAsync(TcpClient client, JObject json)
        {
            var stream = client.GetStream();
            bool success = _authSystem.AuthenticateUser(json.ToString());
            var jsonResponse = "";

            if (success)
            {
                var username = json["username"]?.ToString();
                var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
                var character = user != null ? _dbContext.Characters.FirstOrDefault(c => c.User.Id == user.Id) : null;

                if (character != null)
                {
                    // Register the client with the session manager
                    _sessionManager.RegisterClient(client, character);

                    var otherPlayers = _sessionManager.GetAllConnectedPlayers();
                    var localPlayer = otherPlayers.FirstOrDefault(p => p.Name == character.Name);

                    var initialData = new InitialSyncData
                    {
                        LocalPlayer = localPlayer,
                        OtherPlayers = otherPlayers.Where(p => p.Id != localPlayer.Id).ToList()
                    };

                    jsonResponse = JsonConvert.SerializeObject(initialData);
                    byte[] bytes = Encoding.UTF8.GetBytes(jsonResponse);
                    await stream.WriteAsync(bytes, 0, bytes.Length);

                    PrettyConsole.LogConnection(
                            client.Client.RemoteEndPoint?.ToString() ?? "Unknown"
                        );
                    return;
                }
            }

            ServerMessage message = new ServerMessage
            {
                Message = "Login error."
            };

            jsonResponse = JsonConvert.SerializeObject(message);
            byte[] responseBytes = Encoding.UTF8.GetBytes(jsonResponse.ToString() ?? string.Empty);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}
