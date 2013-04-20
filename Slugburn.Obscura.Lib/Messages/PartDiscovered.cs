using System;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Messaging;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Messages
{
    public class PartDiscovered
    {
        public PartDiscovered(ShipPart part)
        {
            Part = part;
        }

        public ShipPart Part { get; private set; }

        public class FactionHandler : MessageHandler<PlayerFaction>
        {
            protected override IDisposable Subscribe(IMessagePipe pipe)
            {
                return pipe.Subscribe<PartDiscovered>(x =>
                    {
                        Context.DiscoveredParts.Add(x.Part);
                        pipe.Publish(new PartListChanged());
                    });
            }
        }
    }
}
