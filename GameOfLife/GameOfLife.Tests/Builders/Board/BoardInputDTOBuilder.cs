using GameOfLife.DTO.Boards;

namespace GameOfLife.Tests.Builders.Board
{
    public class BoardInputDTOBuilder
    {
        private int _rows;
        private int _columns;
        private List<CellLocationDTO> _aliveCells;

        public BoardInputDTOBuilder()
        {
            _rows = 3;
            _columns = 3;
            _aliveCells = new List<CellLocationDTO>();
        }

        public BoardInputDTOBuilder WithRows(int rows)
        {
            _rows = rows;
            return this;
        }

        public BoardInputDTOBuilder WithColumns(int columns)
        {
            _columns = columns;
            return this;
        }

        public BoardInputDTOBuilder WithAliveCells(List<CellLocationDTO> aliveCells)
        {
            _aliveCells = aliveCells;
            return this;
        }

        public BoardInputDTO Build()
        {
            return new BoardInputDTO
            {
                Rows = _rows,
                Columns = _columns,
                AliveCells = _aliveCells
            };
        }
    }
}
