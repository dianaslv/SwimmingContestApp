using System.Collections.Generic;

namespace Networking.NetworkModels
{
    public class Request
    {
        public RequestType RequestType { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    }
}