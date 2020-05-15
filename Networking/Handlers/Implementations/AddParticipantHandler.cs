using Microsoft.Extensions.Logging;
using Networking.Handlers.Interfaces;
using Networking.NetworkModels;
using Networking.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Handlers.Implementations
{
    class AddParticipantHandler :IAddParticipantHandler
    {
        private readonly ILogger<AddParticipantHandler> m_logger;
        private readonly IParticipantService m_participantService;

        public AddParticipantHandler(ILogger<AddParticipantHandler> logger, IParticipantService ticketService)
        {
            m_logger = logger;
            m_participantService = ticketService;
        }

        public async Task<bool> ProcessAsync(TcpClient client, Guid clientId, Request request)
        {
            try
            {
                var info = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Body);
                await m_participantService.CreateParticipant(Guid.Parse(info["idParticipant"]),info["participantName"], int.Parse(info["participantAge"]));
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
