namespace Slugburn.Obscura.Lib
{
    public class ProductionQuantity
    {
        private readonly int[] _amount;

        public ProductionQuantity()
        {
            _amount=new int[4];
        }

        public int this[ProductionType type]
        {
            get { return _amount[(int) type - 1]; }
            set { _amount[(int) type - 1] = value; }
        }
    }
}