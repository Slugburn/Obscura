using System;

namespace Slugburn.Obscura.Lib.Messaging
{
    public interface IMessageHandler<in TContext> where TContext : class
    {
        IDisposable Subscribe(IMessagePipe pipe, TContext context);
    }
}
