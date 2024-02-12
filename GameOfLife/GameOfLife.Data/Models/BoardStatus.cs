namespace GameOfLife.Data.Models
{
    public enum BoardStatus
    {
        /// <summary>
        /// The board is in an active state.
        /// </summary>
        Active,
        /// <summary>
        /// The last state of the board was eiqual to the previous one.
        /// </summary>
        Finalized,
        /// <summary>
        /// The board reached the maximum number of generations.
        /// </summary>
        Invalid
    }
}
