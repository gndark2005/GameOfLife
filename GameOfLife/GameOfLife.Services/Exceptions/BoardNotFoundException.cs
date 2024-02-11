namespace GameOfLife.Services.Exceptions
{
    public class BoardNotFoundException : Exception
    {
        private const string ERROR_MESSAGE = "Board with Id {0} not found";

        public BoardNotFoundException(Guid boardId)
        : base(string.Format(ERROR_MESSAGE, boardId.ToString()))
        {
        }
    }
}
