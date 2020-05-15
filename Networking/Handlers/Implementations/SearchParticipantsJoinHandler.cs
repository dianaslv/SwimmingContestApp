using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Networking.Handlers.Interfaces;
using Networking.NetworkModels;
using Networking.Services.Interfaces;
using Newtonsoft.Json;

namespace Networking.Handlers.Implementations
{
    public class SearchParticipantsJoin : ISearchParticipantsJoin
    {
        private readonly IParticipantService m_participantService;
        private readonly ILogger<SearchParticipantsHandler> m_logger;

        public SearchParticipantsJoin(IParticipantService participantService, ILogger<SearchParticipantsHandler> logger)
        {
            this.m_participantService = participantService;
            m_logger = logger;
        }

        public async Task<DataSet> ProcessAsync(TcpClient client, Request request)
        {
            try
            {
                var set = new DataSet();
                var info = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Body);
                var adapter = await m_participantService.GetParticipantsDbAdapterWithJoinAsync(Guid.Parse(info["idTask"]));
                adapter.Fill(set);
                return set;
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Issue on participant handler");
                return null;
            }
        }
    }
}