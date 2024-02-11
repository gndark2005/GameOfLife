using GameOfLife.DTO.Boards;

namespace GameOfLife.Services.Boards.Abstractions
{
    public interface IBoardService
    {
        Task<Guid> CreateBoardAsync(BoardInputDTO boardInputDTO);

        Task<BoardOutputDTO> GetBoardCurrentStatusAsync(Guid boardId);

        Task<BoardOutputDTO> GetBoardNextGenerationAsync(Guid boardId);

        Task<BoardOutputDTO> GetBoardNextNumberOfGenerationsAsync(Guid boardId, int NumberOfGenerations);

        Task<BoardOutputDTO> GetBoardUntilFinalizedAsync(Guid boardId);
    }
}
