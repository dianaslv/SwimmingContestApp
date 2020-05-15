using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Networking.NetworkModels;
using Newtonsoft.Json;

namespace Networking.Network
{
    public class Client
    {
        private const int Port = 5050;
        private ILogger<Client> m_logger;
        private const string Address = "127.0.0.1"; 
        private TcpClient ClientSocket { get; set; }
        private readonly SynchronizedCollection<Request> m_responses = new SynchronizedCollection<Request>();
        public Observer Observer { get; set; }
        
        public Client(ILogger<Client> logger)
        {
            m_logger = logger;
        }

        public CancellationTokenSource Cts { get; } = new CancellationTokenSource();


        public void StartClient()
        {
            ClientSocket = new TcpClient(Address, Port);
            new Thread(async () => { await ListenAsync(); }).Start();
        }

        public async Task SendMessageAsync(Request request)
        {
            while (!Cts.IsCancellationRequested)
            {
                var requestData = RequestParser.SerializeRequestAsByte(request);
                var stream = ClientSocket.GetStream();
                await stream.WriteAsync(requestData, 0, requestData.Length);
                m_logger.LogInformation($"Sent Request : {JsonConvert.SerializeObject(request, Formatting.Indented)}");
                await stream.FlushAsync();
                await Task.Delay(100);
                return;
            }
        }

        private async Task ListenAsync()
        {
            while (true)
            {
                var buffer = new byte[4096];
                var stream = ClientSocket?.GetStream();
                if (stream == null || !stream.CanRead)
                    continue;
                await stream.ReadAsync(buffer, 0, buffer.Length, Cts.Token);
                var response = RequestParser.DeserializeRequestClient(buffer);
                if (response == null) 
                    continue;
                if (response.RequestType.Equals(RequestType.DataChanged))
                {
                    m_logger.LogInformation($"Registered Broadcast {JsonConvert.SerializeObject(response, Formatting.Indented)}");
                    Observer.DataChanged = true;
                    continue;
                }
                m_responses.Add(response);
                m_logger.LogInformation($"Registered response {JsonConvert.SerializeObject(response, Formatting.Indented)}");
            }
        }

        public async Task<T> GetResponse<T>(RequestType type)
        {
            var retryCount = 0;
            while (retryCount < 30)
            {
                var response = m_responses.FirstOrDefault(t => t.RequestType.Equals(type));

                if (response == null)
                {
                    await Task.Delay(100);
                    retryCount++;
                    continue;
                }

                m_responses.Remove(response);

                if (!response.Headers["Status"].Equals(nameof(ResponseStatus.Success)))
                    return default;

                return response.Headers["ContentType"].Equals(nameof(RequestContentType.Json))
                    ? JsonConvert.DeserializeObject<T>(response.Body)
                    : default;
            }

            return default;
        }

        public async Task<Response> GetResponse(RequestType type)
        {
            var retryCount = 0;
            var value = new Response
            {
                Message = "No Data"
            };
            while (retryCount < 30)
            {
                var response = m_responses.FirstOrDefault(t => t.RequestType.Equals(type));
                if (response == null)
                {
                    await Task.Delay(100);
                    retryCount++;
                    continue;
                }

                m_responses.Remove(response);
                return new Response
                {
                    Success = response.Headers["Status"].Equals(nameof(ResponseStatus.Success)), Message = response.Body
                };
            }

            return value;
        }
    }
}