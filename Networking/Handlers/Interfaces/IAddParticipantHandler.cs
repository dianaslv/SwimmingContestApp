using Networking.NetworkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Handlers.Interfaces
{
    public interface IAddParticipantHandler
    {
        Task<bool> ProcessAsync(TcpClient client, Guid clientId, Request request);
    }
}
