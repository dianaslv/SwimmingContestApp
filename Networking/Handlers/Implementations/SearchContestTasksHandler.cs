using Networking.Handlers.Interfaces;
using Networking.NetworkModels;
using Networking.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Networking.Handlers.Implementations
{
    class SearchContestTasksHandler : ISearchContestTasksHandler
    {
        private readonly IContestTaskService m_contestTaskService;
        private readonly ILogger<SearchContestTasksHandler> m_logger;
        public SearchContestTasksHandler(IContestTaskService contestTaskService, ILogger<SearchContestTasksHandler> logger)
        {
            this.m_contestTaskService = contestTaskService;
            m_logger = logger;
        }
        public async Task<DataSet> ProcessAsync(TcpClient client, Request request)
        {
            try
            {
                var set = new DataSet();
                var adapter = await m_contestTaskService.GetContestTasksDbAdapter(null);
                adapter.Fill(set);
                return set;
            }
            catch(Exception e)
            {
                m_logger.LogError(e, "Issue on contestaskhandler");
                return null;
            }
        }
    }
}
