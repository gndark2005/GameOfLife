using GameOfLife.Data.Models;
using System.Drawing;
using System.Text.Json;

namespace GameOfLife.Services.Extensions
{
    public static class BoardExtentions
    {
        public static byte[,] GetCells(this Board board)
        {
            var aliveCells = JsonSerializer.Deserialize<IReadOnlyCollection<Point>>(board.AliveCellsJson)
                ?? throw new InvalidOperationException($"AliveCellsJson is null in board {board.Id} ");

            var cells = new byte[board.Rows, board.Columns];

            foreach (var cell in aliveCells)
            {
                cells[cell.X, cell.Y] = 1;
            }

            return cells;
        }
    }
}
