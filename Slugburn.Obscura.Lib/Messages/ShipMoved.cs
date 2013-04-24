using System;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Messaging;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Messages
{
    public class ShipMoved
    {
        public Ship Ship { get; set; }
        public Sector Start { get; set; }
        public Sector End { get; set; }

        public ShipMoved(Ship ship, Sector start, Sector end)
        {
            Ship = ship;
            Start = start;
            End = end;
        }

        public class Handler : MessageHandler<Game>
        {
            protected override IDisposable Subscribe(IMessagePipe pipe)
            {
                return pipe.Subscribe<ShipMoved>(x => Context.View.MoveShip(x.Ship, x.Start, x.End));
            }
        }
    }
}