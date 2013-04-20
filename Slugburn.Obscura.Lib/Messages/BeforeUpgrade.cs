using System;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Messaging;

namespace Slugburn.Obscura.Lib.Messages
{
    public class BeforeUpgrade
    {
        public class Handler : MessageHandler<AiPlayer>
        {
            protected override IDisposable Subscribe(IMessagePipe pipe)
            {
                return pipe.Subscribe<BeforeUpgrade>(
                    x => Context.UpgradeListGenerator.RateFleet(Context));
            }
        }
    }
}