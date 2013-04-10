namespace Slugburn.Obscura.Lib.Ships
{
    public class GalacticCenterDefenseSystem : Ship
    {
        private static readonly ShipProfile _profile = new ShipProfile
            {
                Accuracy = 1,
                Cannons = new[] {1, 1, 1, 1},
                Structure = 7
            };
        public override ShipProfile Profile
        {
            get { return _profile; }
        }
    }
}
