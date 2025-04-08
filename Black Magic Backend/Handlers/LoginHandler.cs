using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Black_Magic_Backend.Handlers
{
    public class LoginHandler : IMessageHandler {
        private readonly AuthSystem _authSystem;

        public LoginHandler(AuthSystem authSystem) {
            _authSystem = authSystem;
        }

        public async Task HandleAsync(UdpClient client, JObject json, IPEndPoint remoteEndPoint) {
            bool success = _authSystem.AuthenticateUser(json.ToString());

            string response = success ? "Login successful!" : "Login error.";
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await client.SendAsync(responseBytes, responseBytes.Length, remoteEndPoint);
        }
    }
}
