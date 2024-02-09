namespace GameOfLife.DTO.Boards
{
    public class BoardOutputDTO
    {
        public Guid BoardId { get; set; }

        public int CurrenGeneration { get; set; }

        public BoardStatusDTO Status { get; set; }

        public byte[,] Cells { get; set; }
    }
}
