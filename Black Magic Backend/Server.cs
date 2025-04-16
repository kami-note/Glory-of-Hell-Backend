using Black_Magic_Backend.Handlers;
using Black_Magic_Backend.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;

namespace Black_Magic_Backend {
    public class Server {
        private readonly Dictionary<string, IMessageHandler> _handlers;
        private readonly UdpClient _udpClient;
        private readonly IPEndPoint _endPoint;

        public Server(string listenAddress = "0.0.0.0", int port = 8080) {
            _endPoint = new IPEndPoint(IPAddress.Any, port);
            _udpClient = new UdpClient(_endPoint);

            var authSystem = new AuthSystem();
            var dbContext = new ApplicationDbContext();
            var clientSessions = new Dictionary<IPEndPoint, ClientSession>();

            _handlers = new Dictionary<string, IMessageHandler> {
                { "register", new RegisterHandler(authSystem) },
                { "login", new LoginHandler(authSystem, dbContext, clientSessions) }
            };
        }

        public async Task StartAsync() {
            Console.WriteLine($"UDP Server listening on {_endPoint}");

            while (true) {
                var result = await _udpClient.ReceiveAsync();
                _ = Task.Run(() => ProcessRequestAsync(result));
            }
        }

        private async Task ProcessRequestAsync(UdpReceiveResult result) {
            string data = Encoding.UTF8.GetString(result.Buffer);

            try {
                JObject json = JObject.Parse(data);

                if (!json.TryGetValue("type", out var typeToken)) {
                    Console.WriteLine("Missing 'type' field");
                    return;
                }

                string type = typeToken.ToString();

                if (_handlers.TryGetValue(type, out var handler)) {
                    await handler.HandleAsync(_udpClient, json, result.RemoteEndPoint);
                } else {
                    Console.WriteLine($"Unknown message type: {type}");
                }
            } catch (Exception ex) {
                Console.WriteLine($"Failed to process data: {ex.Message}");
            }
        }

        public void Stop() {
            _udpClient.Close();
            Console.WriteLine("UDP Server stopped.");
        }
    }
}