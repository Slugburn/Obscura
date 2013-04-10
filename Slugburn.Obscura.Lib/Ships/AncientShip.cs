namespace Slugburn.Obscura.Lib.Ships
{
    public class AncientShip : Ship
    {
        private static readonly ShipProfile _profile = new ShipProfile
            {
                Cannons = new[] {1, 1},
                Accuracy = 1,
                Structure = 1,
                Initiative = 2
            };

        public override ShipProfile Profile
        {
            get { return _profile; }
        }
    }
}
