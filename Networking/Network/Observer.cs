using System;
using System.Threading.Tasks;

namespace Networking.Network
{
    public class Observer
    {
        private bool m_startInvoked;
        public Func<Task> ToInvoke { get; set; }

        public bool DataChanged
        {
            get => m_startInvoked;
            set
            {
                m_startInvoked = value;
                ToInvoke.Invoke();
                m_startInvoked = false;
            }
        }
    }
}