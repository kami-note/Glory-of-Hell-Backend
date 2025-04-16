using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Text;

namespace Black_Magic_Backend.Handlers {
    public class RegisterHandler : IMessageHandler {
        private readonly AuthSystem _authSystem;

        public RegisterHandler(AuthSystem authSystem) {
            _authSystem = authSystem;
        }

        public async Task HandleAsync(TcpClient client, JObject json) {
            var stream = client.GetStream();
            bool success = _authSystem.RegisterUser(json.ToString());

            string response = success ? "Registration done!" : "Error in registration.";
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}
