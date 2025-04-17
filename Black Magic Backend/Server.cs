using Black_Magic_Backend.Handlers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Black_Magic_Backend {
    public class Server {
        private readonly Dictionary<string, IMessageHandler> _handlers;
        private readonly TcpListener _tcpListener;

        public Server(string listenAddress = "0.0.0.0", int port = 8080) {
            var ip = IPAddress.Parse(listenAddress);
            _tcpListener = new TcpListener(ip, port);

            var authSystem = new AuthSystem();
            var dbContext = new ApplicationDbContext();
            var connectedClients = new Dictionary<TcpClient, ClientSession>();
            var sessionManager = new SessionManager(connectedClients);

            _handlers = new Dictionary<string, IMessageHandler> {
                { "register", new RegisterHandler(authSystem) },
                { "login", new LoginHandler(authSystem, dbContext, sessionManager) }
            };
        }

        public async Task StartAsync() {
            _tcpListener.Start();
            PrettyConsole.LogInfo("TCP Server listening...");

            while (true) {
                var client = await _tcpListener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client) {
            var stream = client.GetStream();
            var buffer = new byte[4096];

            while (client.Connected) {
                int bytesRead;
                try {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                } catch {
                    PrettyConsole.LogWarning("Client disconnected.");
                    break;
                }

                if (bytesRead == 0) break;

                var data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                try {
                    JObject json = JObject.Parse(data);

                    if (!json.TryGetValue("action", out var actionToken)) {
                        PrettyConsole.LogWarning("Missing 'action' field");
                        continue;
                    }

                    string action = actionToken.ToString();

                    if (_handlers.TryGetValue(action, out var handler)) {
                        await handler.HandleAsync(client, json);
                    } else {
                        PrettyConsole.LogWarning($"Unknown message action: {action}");
                    }
                } catch (Exception ex) {
                    PrettyConsole.LogError($"Failed to process data: {ex.Message}");
                }
            }

            client.Close();
            PrettyConsole.LogInfo("Closed client connection.");
        }

        public void Stop() {
            _tcpListener.Stop();
            PrettyConsole.LogInfo("TCP Server stopped.");
        }
    }
}
