using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.Extensions.Logging;
using Networking.Handlers.Interfaces;
using Networking.NetworkModels;
using Networking.Services.Interfaces;
using Newtonsoft.Json;

namespace Networking.Handlers.Implementations
{
    public class LoginHandler : ILoginHandler
    {
        private readonly IUserService m_userService;
        private readonly ILogger<LoginHandler> m_logger;

        public LoginHandler(IUserService userService, ILogger<LoginHandler> logger)
        {
            m_userService = userService;
            m_logger = logger;
        }

        public async Task<User> ProcessAsync(TcpClient client, Request request)
        {
            var info = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.Body);
            var loggedUser = await m_userService.LoginUserAsync(info["username"], info["password"]);
            if (loggedUser == null)
            {
                m_logger.LogWarning($"Could not find any user with credentials {info["username"]} {info["password"]}");
            }

            return loggedUser;
        }
    }
}