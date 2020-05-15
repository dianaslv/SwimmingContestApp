using System.Net.Sockets;
using System.Threading.Tasks;
using Core.Models;
using Networking.NetworkModels;

namespace Networking.Handlers.Interfaces
{
    public interface ILoginHandler
    {
        Task<User> ProcessAsync(TcpClient client, Request request);
    }
}