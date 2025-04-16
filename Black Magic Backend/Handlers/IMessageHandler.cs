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
    public interface IMessageHandler
    {
        Task HandleAsync(TcpClient client, JObject json);
    }
}
