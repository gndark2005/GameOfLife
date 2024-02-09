using System.Drawing;

namespace GameOfLife.DTO.Boards
{
    public  class BoardInputDTO
    {
        public int Rows { get; set; }

        public int Cols { get; set; }

        public IReadOnlyCollection<Point> AliveCells { get; set; }
    }
}
