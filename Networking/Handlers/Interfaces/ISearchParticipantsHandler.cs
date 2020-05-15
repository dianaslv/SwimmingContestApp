using System.Data;
using System.Net.Sockets;
using System.Threading.Tasks;
using Networking.NetworkModels;

namespace Networking.Handlers.Interfaces
{
    public interface ISearchParticipantsHandler
    {
        Task<DataSet> ProcessAsync(TcpClient client, Request request);
    }
}