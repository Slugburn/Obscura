using System;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Messaging;

namespace Slugburn.Obscura.Lib.Messages
{
    public class SectorUpdated
    {
        public Sector Sector { get; set; }

        public SectorUpdated(Sector sector)
        {
            Sector = sector;
        }

        public class Handler : MessageHandler<Game>
        {
            protected override IDisposable Subscribe(IMessagePipe pipe)
            {
                return pipe.Subscribe<SectorUpdated>(x => Context.View.UpdateSector(x.Sector));
            }
        }
    }
}