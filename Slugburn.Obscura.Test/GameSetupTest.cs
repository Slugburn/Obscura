﻿using System.Linq;
using NUnit.Framework;
using Ninject;
using Ninject.Extensions.Conventions;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Test
{
    [TestFixture]
    public class GameSetupTest
    {
        [Test]
        public void Start()
        {
            // Arrange
            var kernel = new StandardKernel();
            kernel.Load(new TestModule());
            var game = kernel.Get<Game>();
            var factions = Enumerable.Range(0, 6).Select(x => kernel.Get<Faction>()).ToArray();
            
            // Act
            game.Setup(factions);
            game.Start();
        }

    }
}
