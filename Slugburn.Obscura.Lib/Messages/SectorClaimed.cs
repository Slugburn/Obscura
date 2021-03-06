using System;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Messaging;

namespace Slugburn.Obscura.Lib.Messages
{
    public class SectorClaimed
    {
        public SectorClaimed(Sector sector)
        {
            Sector = sector;
        }

        public Sector Sector { get; private set; }

        public class Handler : MessageHandler<AiPlayer>
        {
            protected override IDisposable Subscribe(IMessagePipe pipe)
            {
                // colonize immediately after claiming a sector
                return pipe.Subscribe<SectorClaimed>(x =>
                    {
                        Context.Faction.Colonize();
                        Context.Faction.Game.SendMessage(new SectorUpdated(x.Sector));
                    });
            }
        }

    }
}