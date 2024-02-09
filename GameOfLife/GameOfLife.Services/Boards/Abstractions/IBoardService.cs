using GameOfLife.DTO.Boards;

namespace GameOfLife.Services.Boards.Abstractions
{
    public interface IBoardService
    {
        public Task<Guid> CreateBoardAsync(BoardInputDTO boardInputDTO);

        public Task<BoardOutputDTO> GetBoardNextGenerationAsync(Guid boardId);

        public Task<BoardOutputDTO> GetBoardNextNumberOfGenerationsAsync(Guid boardId, int NumberOfGenerations);

        public Task<BoardOutputDTO> GetBoardUntilFinalizedAsync(Guid boardId);
    }
}
