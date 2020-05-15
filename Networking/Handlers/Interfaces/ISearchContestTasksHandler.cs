using Networking.NetworkModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Handlers.Interfaces
{
    public interface ISearchContestTasksHandler
    {
        Task<DataSet> ProcessAsync(TcpClient client, Request request);
    }
}
