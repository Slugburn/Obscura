using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Slugburn.Obscura.Lib;

namespace Slugburn.Obscura.Test
{
    [TestFixture]
    public class GameSetupTest
    {
        [Test]
        public void Setup()
        {
            // Arrange
            var game = new Game();
            var players = new[] {new Player(), new Player()};

            // Act
            game.Setup(players);


        }

        [Test]
        public void Start()
        {
            // Arrange
            var game = new Game();
            var players = new[] { new Player(), new Player() };

            // Act
            game.Setup(players);
            game.Start();
        }

    }
}
