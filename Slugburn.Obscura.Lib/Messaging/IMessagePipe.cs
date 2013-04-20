using System;

namespace Slugburn.Obscura.Lib.Messaging
{
    public interface IMessagePipe
    {
        void Publish<TEvent>(TEvent ev);
        IDisposable Subscribe<TEvent>(Action<TEvent> action);
    }
}
