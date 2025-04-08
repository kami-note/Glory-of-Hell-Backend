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
    public class RegisterHandler : IMessageHandler {
        private readonly AuthSystem _authSystem;

        public RegisterHandler(AuthSystem authSystem) {
            _authSystem = authSystem;
        }

        public async Task HandleAsync(UdpClient client, JObject json, IPEndPoint remoteEndPoint) {
            bool success = _authSystem.RegisterUser(json.ToString());

            string response = success ? "Registration done!" : "Error in registration.";
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await client.SendAsync(responseBytes, responseBytes.Length, remoteEndPoint);
        }
    }
}
