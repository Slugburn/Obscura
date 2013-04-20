using System;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Messaging;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Messages
{
    public class BeforeUpgradeToDiscoveredPart
    {
        public ShipPart Part { get; set; }

        public BeforeUpgradeToDiscoveredPart(ShipPart part)
        {
            Part = part;
        }

        public class Handler : MessageHandler<AiPlayer>
        {
            protected override IDisposable Subscribe(IMessagePipe pipe)
            {
                return pipe.Subscribe<BeforeUpgradeToDiscoveredPart>(
                    x => Context.UpgradeList = Context.UpgradeListGenerator.GenerateForDiscoveredPart(Context, x.Part));
            }
        }
    }
}