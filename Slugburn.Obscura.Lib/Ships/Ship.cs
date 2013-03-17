namespace Slugburn.Obscura.Lib.Ships
{
    public class Ship
    {

        public static Ship FromBlueprint(ShipBlueprint blueprint)
        {
            return new PlayerShip(blueprint);
        }
    }

    public class PlayerShip : Ship
    {
        private readonly ShipBlueprint _blueprint;

        public PlayerShip(ShipBlueprint blueprint)
        {
            _blueprint = blueprint;
        }
    }
}
