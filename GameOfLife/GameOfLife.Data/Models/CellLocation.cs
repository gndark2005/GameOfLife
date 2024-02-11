namespace GameOfLife.Data.Models
{
    public class CellLocation
    {
        public int X { get; set; }

        public int Y { get; set; }

        public CellLocation(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
