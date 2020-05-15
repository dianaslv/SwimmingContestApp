using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.Extensions.Logging;
using Networking.Handlers.Interfaces;
using Networking.NetworkModels;
using Newtonsoft.Json;

namespace Networking.Network
{
    public class Server : MarshalByRefObject
    {
        private const int Port = 5050;
        private const string Address = "127.0.0.1";
        private const short ServerSize = 10;
        private readonly TcpListener m_tcpListener;
        private readonly ILogger<Server> m_logger;
        private readonly ILoginHandler m_loginHandler;
        private readonly ISearchContestTasksHandler m_searchContestTasksHandler;
        private readonly ISearchParticipantsHandler m_searchParticipantHandler;
        private readonly ISearchParticipantsJoin m_searchParticipantJoinHandler;
        private readonly IAddParticipantHandler m_addParticipantHandler;
        private readonly IAddEntryHandler m_addEntryHandler;
        private ConcurrentDictionary<Guid, TcpClient> Users { get; } = new ConcurrentDictionary<Guid, TcpClient>();

        public Server(ILoginHandler loginHandler, ILogger<Server> logger, ISearchContestTasksHandler searchContestTasksHandler, IAddParticipantHandler addParticipantHandler, IAddEntryHandler addEntryHandler, ISearchParticipantsHandler searchParticipantHandler,
            ISearchParticipantsJoin searchParticipantJoinHandler)
        {
            m_loginHandler = loginHandler;
            m_logger = logger;
            m_searchContestTasksHandler = searchContestTasksHandler;
            m_addParticipantHandler = addParticipantHandler;
            m_addEntryHandler = addEntryHandler;
            m_searchParticipantHandler = searchParticipantHandler;
            m_searchParticipantJoinHandler = searchParticipantJoinHandler;
            var ipAddress = IPAddress.Parse(Address);
            m_tcpListener = new TcpListener(ipAddress, Port);
            m_tcpListener.Start();
        }

        public async Task StartServer()
        {
            await HandleClients(ServerSize);
        }

        public void CloseServer()
        {
            m_tcpListener.Stop();
        }

        private async Task<TcpClient> ListenAsync()
        {
            var client = await m_tcpListener.AcceptTcpClientAsync();
            return client;
        }

        private async Task HandleClients(int? totalClients)
        {
            var tasks = new List<Task>();
            new Thread(async () => { await StartToServeAsync(tasks); }).Start();

            while (true)
            {
                await AcceptNewClientAsync(totalClients, tasks);
            }
        }

        private static async Task StartToServeAsync(IReadOnlyCollection<Task> clientTasks)
        {
            while (true)
            {
                if (clientTasks.Count == 0)
                    continue;
                var handleClientMessage = await Task.WhenAny(clientTasks);
                new Thread(async () => await handleClientMessage).Start();
            }
        }

        private async Task AcceptNewClientAsync(int? totalClients, ICollection<Task> tasks)
        {
            var newClient = await ListenAsync();

            if (!totalClients.HasValue || tasks.Count < totalClients.Value)
            {
                tasks.Add(GetClientMessage(newClient));
            }
            else
            {
                var request = RequestParser.SerializeRequestAsByte(new Request
                {
                    Body = "Server Is Full, Please Try Later",
                    Headers = null,
                    RequestType = RequestType.Logout
                });
                await newClient.GetStream().FlushAsync();
                await newClient.GetStream().WriteAsync(request, 0, request.Length);
                Console.WriteLine("A client tried to connect but server is full");
            }
        }

        private async Task GetClientMessage(TcpClient client)
        {
            while (true)
            {
                var buffer = new byte[4096];
                var stream = client.GetStream();
                await stream.ReadAsync(buffer, 0, buffer.Length);
                Request request;
                try
                {
                    request = RequestParser.DeserializeRequest(buffer);
                    if (request == null)
                        continue;
                    client.GetStream().Flush();
                    Console.WriteLine($"Received Request {JsonConvert.SerializeObject(request)}");
                    new Thread(async () => await ProcessRequest(request, client)).Start();
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }

        private static async Task SendClientMessage(TcpClient client, Request request)
        {
            var stream = client.GetStream();
            var data = RequestParser.SerializeRequestAsByte(request);
            Console.WriteLine(data.Length);
            await stream.WriteAsync(data, 0, data.Length);
        }

        private async Task ProcessRequest(Request request, TcpClient client)
        {
            switch (request.RequestType)
            {
                case RequestType.Login:
                    await HandleLoginRequestAsync(request, client);
                    break;
                case RequestType.Logout:
                    await HandleLogoutRequestAsync(request, client);
                    break;
                case RequestType.SearchContestTasks:
                    await HandleSearchContestTasksAsync(request, client);
                    break;
                case RequestType.SearchParticipants:
                    await HandleSearchParticipantsAsync(request, client);
                    break;
                case RequestType.SearchParticipantsJoin:
                    await HandleSearchParticipantsJoinAsync(request, client);
                    break;
                case RequestType.AddParticipant:
                    await HandleAddParticipantAsync(request, client);
                    break;
                case RequestType.AddEntry:
                    await HandleAddEntryAsync(request, client);
                    break;
                case RequestType.DataChanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private async Task HandleSearchParticipantsAsync(Request request, TcpClient client)
        {
            var data = await m_searchParticipantHandler.ProcessAsync(client, request);
            if (data != null)
            {
                await CreateAndSendMessageAsync(client, data, RequestType.SearchParticipants, ResponseStatus.Success, RequestContentType.Json);
            }
            else
            {
                await CreateAndSendMessageAsync(client, new {FailReason = "Could not find data"}, RequestType.SearchParticipants, ResponseStatus.Failure, RequestContentType.Json);
            }
        }

        private async Task HandleSearchParticipantsJoinAsync(Request request, TcpClient client)
        {
            var data = await m_searchParticipantJoinHandler.ProcessAsync(client, request);
            if (data != null)
            {
                await CreateAndSendMessageAsync(client, data, RequestType.SearchParticipantsJoin, ResponseStatus.Success, RequestContentType.Json);
            }
            else
            {
                await CreateAndSendMessageAsync(client, new {FailReason = "Could not find data"}, RequestType.SearchParticipantsJoin, ResponseStatus.Failure, RequestContentType.Json);
            }
        }

        private async Task HandleAddEntryAsync(Request request, TcpClient client)
        {
            if (await m_addEntryHandler.ProcessAsync(client, Users.FirstOrDefault(t => t.Value.Equals(client)).Key, request))
            {
                await CreateAndSendMessageAsync(client, new
                {
                    Message = "Entry added"
                }, RequestType.AddEntry, ResponseStatus.Success, RequestContentType.Text);
                return;
            }

            await CreateAndSendMessageAsync(client, new
            {
                Message = "Could not add Entry"
            }, RequestType.AddEntry, ResponseStatus.Failure, RequestContentType.Text);
        }

        private async Task HandleAddParticipantAsync(Request request, TcpClient client)
        {
            if (await m_addParticipantHandler.ProcessAsync(client, Users.FirstOrDefault(t => t.Value.Equals(client)).Key, request))
            {
                await CreateAndSendMessageAsync(client, new
                {
                    Message = "Participant added"
                }, RequestType.AddParticipant, ResponseStatus.Success, RequestContentType.Text);
                await BroadcastMessageAsync(client);
                return;
            }

            await CreateAndSendMessageAsync(client, new
            {
                Message = "Could not add person"
            }, RequestType.AddParticipant, ResponseStatus.Failure, RequestContentType.Text);
        }

        private static async Task CreateAndSendMessageAsync(TcpClient client, object bodyValue, RequestType type, ResponseStatus status, RequestContentType contentType)
        {
            await client.GetStream().FlushAsync();
            await SendClientMessage(client, new Request
            {
                Headers = new Dictionary<string, string>
                {
                    {"Status", status.ToString()},
                    {"ContentType", contentType.ToString()}
                },
                Body = $"{JsonConvert.SerializeObject(bodyValue)}",
                RequestType = type
            });
            await client.GetStream().FlushAsync();
        }

        private static async Task SendDatasetAsync(TcpClient client, DataSet dataSet, RequestType type)
        {
            if (!dataSet.Equals(null))
            {
                await CreateAndSendMessageAsync(client, dataSet, type, ResponseStatus.Success, RequestContentType.Json);
            }
            else
            {
                await CreateAndSendMessageAsync(client, new {Message = "Could not fetch data from DB"}, type, ResponseStatus.Failure, RequestContentType.Json);
            }
        }

        private async Task BroadcastMessageAsync(TcpClient client)
        {
            foreach (var clientPair in Users)
            {
                if (!clientPair.Value.Equals(client))
                    await CreateAndSendMessageAsync(clientPair.Value, new { }, RequestType.DataChanged, ResponseStatus.Success, RequestContentType.Text);
            }
        }

        private async Task HandleSearchContestTasksAsync(Request request, TcpClient client)
        {
            var data = await m_searchContestTasksHandler.ProcessAsync(client, request);
            if (data != null)
            {
                await CreateAndSendMessageAsync(client, data, RequestType.SearchContestTasks, ResponseStatus.Success, RequestContentType.Json);
            }
            else
            {
                await CreateAndSendMessageAsync(client, new {FailReason = "Could not find data"}, RequestType.SearchContestTasks, ResponseStatus.Failure, RequestContentType.Json);
            }
        }

        private async Task HandleLoginRequestAsync(Request request, TcpClient client)
        {
            var loggedUser = await m_loginHandler.ProcessAsync(client, request);
            if (!loggedUser.Equals(null) && Users.TryAdd(loggedUser.Id, client))
            {
                await CreateAndSendMessageAsync(client, loggedUser, RequestType.Login, ResponseStatus.Success, RequestContentType.Json);
            }
            else
            {
                await CreateAndSendMessageAsync(client, new {Message = "Could not login, is the user already logged?"}, RequestType.Login, ResponseStatus.Failure, RequestContentType.Text);
            }
        }

        private async Task HandleLogoutRequestAsync(Request request, TcpClient client)
        {
            Users.TryRemove(Guid.Parse(request.Body), out var _);
            await CreateAndSendMessageAsync(client, new
            {
                Message = "User Logged out"
            }, RequestType.Logout, ResponseStatus.Success, RequestContentType.Text);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}