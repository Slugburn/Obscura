using System;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Messaging;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Messages
{
    public class TechGained
    {
        public TechGained(Tech tech)
        {
            Tech = tech;
        }

        public Tech Tech { get; private set; }

        public class Handle : MessageHandler<AiPlayer>
        {
            protected override IDisposable Subscribe(IMessagePipe pipe)
            {
                return pipe.Subscribe<TechGained>(x =>
                    {
                        if (x.Tech is PartTech)
                            Context.Faction.SendMessage(new PartListChanged());
                    });
            }
        }
    }
}