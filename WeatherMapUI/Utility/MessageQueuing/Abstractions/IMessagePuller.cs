using System;
using System.Collections.Generic;

namespace MessageQueuing.Abstractions
{
    public interface IMessagePuller
    {
        void Pull(string queueName, Action<IDictionary<string, object>, string> onRecieved);
    }
}
