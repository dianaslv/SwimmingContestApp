using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Networking.NetworkModels;

namespace Networking.Handlers.Interfaces
{
    public interface IAddEntryHandler
    {
        Task<bool> ProcessAsync(TcpClient client, Guid clientId, Request request);
    }
}