using System;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
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

        public class FactionHandler : MessageHandler<Faction>
        {
            protected override IDisposable Subscribe(IMessagePipe pipe)
            {
                return pipe.Subscribe<PartDiscovered>(
                    x =>
                        {
                            Context.DiscoveredParts.Add(x.Part);
                            pipe.Publish(new PartListChanged());
                            var upgradeableBlueprints = Context.Blueprints.Where(bp => bp.CanUsePartToUpgrade(x.Part)).ToArray();
                            if (upgradeableBlueprints.Any())
                                Context.GetAction<UpgradeAction>().UpgradeToDiscoveredPart(Context, x.Part);
                        });
            }
        }
    }
}
