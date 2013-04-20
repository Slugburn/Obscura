using System;

namespace Slugburn.Obscura.Lib.Messaging
{
    public abstract class MessageHandler<TContext> : IMessageHandler<TContext> where TContext : class
    {
        protected TContext Context { get; private set; }

        public IDisposable Subscribe(IMessagePipe pipe, TContext context)
        {
            Context = context;
            return Subscribe(pipe);
        }

        protected abstract IDisposable Subscribe(IMessagePipe pipe);
    }
}
