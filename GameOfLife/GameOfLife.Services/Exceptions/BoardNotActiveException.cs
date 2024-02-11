namespace GameOfLife.Services.Exceptions
{
    public class BoardNotActiveException : Exception
    {
        private const string ERROR_MESSAGE = "Board with Id {0} is not longer active.";

        public BoardNotActiveException(Guid boardId)
        : base(string.Format(ERROR_MESSAGE, boardId.ToString()))
        {
        }
    }
}
