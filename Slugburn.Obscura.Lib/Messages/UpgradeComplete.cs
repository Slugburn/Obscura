using System;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Messaging;

namespace Slugburn.Obscura.Lib.Messages
{
    public class UpgradeComplete
    {

        public class Handler : MessageHandler<AiPlayer>
        {
            protected override IDisposable Subscribe(IMessagePipe pipe)
            {
                return pipe.Subscribe<UpgradeComplete>(x => Context.UpgradeList.RemoveAt(0));
            }
        }
    }
}
