using GameOfLife.Data.Models;

namespace GameOfLife.Services.Extensions
{
    public static class ArrayExtensions
    {
        public static bool AreEqual(this byte[,] array1, byte[,] array2)
        {
            if (array1.GetLength(0) != array2.GetLength(0) || array1.GetLength(1) != array2.GetLength(1))
            {
                return false;
            }

            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    if (array1[i, j] != array2[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static IEnumerable<CellLocation> GetAliveCells(this byte[,] cells)
        {
            var aliveCells = new List<CellLocation>();

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] == 1)
                    {
                        aliveCells.Add(new CellLocation(x, y));
                    }
                }
            }

            return aliveCells;
        }
    }
}
