namespace GameOfLife.DTO.Boards
{
    public class BoardOutputDTO
    {
        public Guid BoardId { get; set; }

        public int CurrentGeneration { get; set; }

        public BoardStatusDTO Status { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public IEnumerable<CellLocationDTO> AliveCells { get; set; }
    }
}
