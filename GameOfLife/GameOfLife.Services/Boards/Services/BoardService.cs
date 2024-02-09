using AutoMapper;
using FluentValidation;
using GameOfLife.Data.Models;
using GameOfLife.Data.Repositories.Abstractions;
using GameOfLife.DTO.Boards;
using GameOfLife.Services.Boards.Abstractions;
using GameOfLife.Services.Cache.Abstractions;
using GameOfLife.Services.Extensions;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Text.Json;

namespace GameOfLife.Services.Boards.Services
{
    public class BoardService : IBoardService
    {
        private const int MAX_GENERATIONS = 1000000;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IValidator<BoardInputDTO> _validator;
        private readonly ILogger<BoardService> _logger;
        private readonly IMapper _mapper;

        public BoardService(
            IUnitOfWork unitOfWork,
            IMemoryCacheService memoryCacheService,
            IValidator<BoardInputDTO> validator,
            ILogger<BoardService> logger)
        {
            _unitOfWork = unitOfWork;
            _memoryCacheService = memoryCacheService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Guid> CreateBoardAsync(BoardInputDTO boardInputDTO)
        {
            await AssertValidBoardInputDTOAsync(boardInputDTO);

            var board = new Board
            {
                Id = Guid.NewGuid(),
                Status = BoardStatus.Active,
                Rows = boardInputDTO.Rows,
                Columns = boardInputDTO.Columns,
                AliveCellsJson = JsonSerializer.Serialize(boardInputDTO.AliveCells),
                CreationDatetime = DateTime.UtcNow,
                LastUpdateDatetime = DateTime.UtcNow,
                CurrentGeneration = 0
            };

            await _unitOfWork.BoardRepository.InsertAsync(board);
            _memoryCacheService.AddToCache(board.Id.ToString(), board);

            _logger.LogInformation("Board with Id {board.Id} created", board.Id);

            return board.Id;
        }

        public async Task<BoardOutputDTO> GetBoardNextGenerationAsync(Guid boardId)
        {
            var board = await GetBoardAsync(boardId);
            var cells = board.GetCells();

            if (board.Status != BoardStatus.Active) throw new InvalidOperationException($"Board with Id {boardId} is not active");

            cells = CalculateNextGeneration(cells);

            board.CurrentGeneration++;
            board.LastUpdateDatetime = DateTime.UtcNow;
            board.AliveCellsJson = JsonSerializer.Serialize(cells.GetAliveCells());

            _unitOfWork.BoardRepository.Update(board);
            _memoryCacheService.AddToCache(board.Id.ToString(), board);

            var result = new BoardOutputDTO
            {
                BoardId = board.Id,
                CurrenGeneration = board.CurrentGeneration,
                Status = (BoardStatusDTO)board.Status,
                Cells = cells
            };

            return result;
        }

        public async Task<BoardOutputDTO> GetBoardNextNumberOfGenerationsAsync(Guid boardId, int NumberOfGenerations)
        {
            var board = await GetBoardAsync(boardId);
            var cells = board.GetCells();

            if (board.Status != BoardStatus.Active) throw new InvalidOperationException($"Board with Id {boardId} is not active");

            for (int i = 0; i < NumberOfGenerations; i++)
            {
                cells = CalculateNextGeneration(cells);
            }

            board.CurrentGeneration += NumberOfGenerations;
            board.LastUpdateDatetime = DateTime.UtcNow;
            board.AliveCellsJson = JsonSerializer.Serialize(cells.GetAliveCells());

            _unitOfWork.BoardRepository.Update(board);
            _memoryCacheService.AddToCache(board.Id.ToString(), board);

            var result = _mapper.Map<BoardOutputDTO>(board);
            result.Cells = cells;

            return result;
        }

        public async Task<BoardOutputDTO> GetBoardUntilFinalizedAsync(Guid boardId)
        {
            var board = await GetBoardAsync(boardId);
            var cells = board.GetCells();

            while (true)
            {
                var newCells = CalculateNextGeneration(cells);

                if (newCells.AreEqual(cells))
                {
                    board.Status = BoardStatus.Finalized;
                    break;
                }
                else
                {
                    cells = newCells;
                    board.CurrentGeneration++;

                    if (board.CurrentGeneration > MAX_GENERATIONS)
                    {
                        board.Status = BoardStatus.Invalid;
                        break;
                    }
                }
            }

            board.LastUpdateDatetime = DateTime.UtcNow;
            board.AliveCellsJson = JsonSerializer.Serialize(cells.GetAliveCells());

            _unitOfWork.BoardRepository.Update(board);
            _memoryCacheService.AddToCache(board.Id.ToString(), board);

            if (board.Status == BoardStatus.Invalid)
            {
                throw new OverflowException($"Board with Id {board.Id} has exceded maximum generation.");
            }

            var result = _mapper.Map<BoardOutputDTO>(board);
            result.Cells = cells;
            //var result = new BoardOutputDTO
            //{
            //    BoardId = board.Id,
            //    CurrenGeneration = board.CurrentGeneration,
            //    Status = (BoardStatusDTO)board.Status,
            //    Cells = cells
            //};

            return result;
        }

        private async Task<Board> GetBoardAsync(Guid boardId)
        {
            var board = _memoryCacheService.GetFromCache<Board>(boardId.ToString());
            board ??= await _unitOfWork.BoardRepository.GetByIdAsync(boardId);

            if (board == null)
            {
                throw new InvalidOperationException($"Board with Id {boardId} not found");
            }

            return board;
        }

        private async Task AssertValidBoardInputDTOAsync(BoardInputDTO boardInputDTO)
        {
            var validationResult = await _validator.ValidateAsync(boardInputDTO);

            if (!validationResult.IsValid)
            {
                _logger.LogError(message: "Validation of BoardInputDTO failed");
                validationResult.Errors.ForEach(error =>
                {
                    _logger.LogError(message: error.ToString());
                });

                throw new ValidationException(validationResult.Errors);
            }
        }

        private static byte[,] CalculateNextGeneration(byte[,] cells)
        {
            var rows = cells.GetLength(0);
            var cols = cells.GetLength(1);
            var newCells = new byte[rows, cols];
            var aliveCells = new List<Point>();

            var getAliveNeigbors = new Func<Point, int>(cell =>
            {
                var aliveNeighbors = 0;

                for (int x = cell.X - 1; x <= cell.X + 1; x++)
                    for (int y = cell.Y - 1; y <= cell.Y + 1; y++)
                        if (x >= 0 && x < rows && y >= 0 && y < cols)
                            aliveNeighbors += cells[x, y];

                return aliveNeighbors;
            });

            var calculateNextStatus = new Func<Point, byte>(cell =>
            {
                var aliveNeighbors = getAliveNeigbors(cell);

                if (aliveNeighbors < 2 || aliveNeighbors > 3)
                    return 0;
                else if (aliveNeighbors == 3)
                    return 1;
                else
                    return cells[cell.X, cell.Y];
            });

            Parallel.ForEach(GetAllCells(rows, cols), cell =>
            {
                newCells[cell.X, cell.Y] = calculateNextStatus(cell);
            });

            return newCells;
        }

        private static IEnumerable<Point> GetAllCells(int rows, int cols)
        {
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }
    }
}
