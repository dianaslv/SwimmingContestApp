using System;
using System.Text;
using Networking.NetworkModels;
using Newtonsoft.Json;

namespace Networking.Network
{
    public static class RequestParser
    {
        public static byte[] SerializeRequestAsByte(Request request)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
        }

        public static Request DeserializeRequest(byte[] data)
        {
            try
            {
                return JsonConvert.DeserializeObject<Request>(Encoding.UTF8.GetString(data));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Request DeserializeRequestClient(byte[] data)
        {
            try
            {
                return JsonConvert.DeserializeObject<Request>(Encoding.UTF8.GetString(data));
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}