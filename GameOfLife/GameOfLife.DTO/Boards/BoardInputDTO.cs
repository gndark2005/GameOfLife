namespace GameOfLife.DTO.Boards
{
    public  class BoardInputDTO
    {
        public int Rows { get; set; }

        public int Columns { get; set; }

        public IReadOnlyCollection<CellLocationDTO> AliveCells { get; set; }
    }
}
