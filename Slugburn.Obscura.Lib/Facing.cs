namespace Slugburn.Obscura.Lib
{
    public class Facing
    {
        public static int[] All = new[] {2, 4, 6, 8, 10, 12};

        public static int Reverse(int facing)
        {
            return RotateClockwise(facing, 6);
        }

        public static int RotateClockwise(int facing, int hours)
        {
            var reverse = facing + hours;
            return reverse <= 12 ? reverse : reverse - 12;
        }
    }
}