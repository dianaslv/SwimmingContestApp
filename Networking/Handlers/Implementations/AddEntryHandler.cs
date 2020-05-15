using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Networking.Handlers.Interfaces;
using Networking.NetworkModels;
using Networking.Services.Interfaces;
using Newtonsoft.Json;

namespace Networking.Handlers.Implementations
{
    public class AddEntryHandler : IAddEntryHandler
    {
        private readonly ILogger<AddEntryHandler> m_logger;
        private readonly IEntryService m_EntryService;

        public AddEntryHandler(ILogger<AddEntryHandler> logger, IEntryService ticketService)
        {
            m_logger = logger;
            m_EntryService = ticketService;
        }

        public async Task<bool> ProcessAsync(TcpClient client, Guid clientId, Request request)
        {
            try
            {
                var info = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Body);
                await m_EntryService.CreateEntry(Guid.Parse(info["idParticipant"]),Guid.Parse(info["contestTasksIdsParticipant"]));
                return true;
            }
            catch (Exception e)
            {
                m_logger.LogError($"Could not add to DataBase : {request.Body}");
                return false;
            }
        }
    }
}