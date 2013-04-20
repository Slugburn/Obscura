using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace Slugburn.Obscura.Lib.Messaging
{
    public static class MessageExtensions
    {
        public static CompositeDisposable Configure<T>(this IEnumerable<IMessageHandler<T>> messageHandlers, T context, IMessagePipe messagePipe)
            where T: class
        {
            return new CompositeDisposable(messageHandlers.Select(x => x.Configure(context, messagePipe)));
        }

        private static IDisposable Configure<T>(this IMessageHandler<T> handler, T context, IMessagePipe messagePipe) where T : class
        {
            return handler.Subscribe(messagePipe, context);
        }
    }
}
