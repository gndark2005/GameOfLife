namespace GameOfLife.DTO.Boards
{
    public class CellLocationDTO
    {
        public int X { get; set; }

        public int Y { get; set; }

        public CellLocationDTO() { }

        public CellLocationDTO(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (CellLocationDTO)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
