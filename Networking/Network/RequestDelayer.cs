﻿using System.Threading.Tasks;

 namespace Networking.Network
{
    public class RequestDelayer
    {
        public int Miliseconds { get; set; }

        public async Task Wait()
        {
            while (Miliseconds > 0)
            {
                await Task.Delay(1);
                Miliseconds -= 1;
            }
        }
    }
}