using AutoMapper;
using FluentValidation;
using GameOfLife.Data.Models;
using GameOfLife.Data.Repositories.Abstractions;
using GameOfLife.DTO.Boards;
using GameOfLife.Services.Boards.Abstractions;
using GameOfLife.Services.Boards.Configuration;
using GameOfLife.Services.Cache.Abstractions;
using GameOfLife.Services.Exceptions;
using GameOfLife.Services.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GameOfLife.Services.Boards.Services
{
    public class BoardService : IBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IValidator<BoardInputDTO> _validator;
        private readonly ILogger<BoardService> _logger;
        private readonly BoardSettings _boardSettings;
        private readonly IMapper _mapper;

        public BoardService(
            IUnitOfWork unitOfWork,
            IMemoryCacheService memoryCacheService,
            IValidator<BoardInputDTO> validator,
            ILogger<BoardService> logger,
            IOptions<BoardSettings> boardSettings,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _memoryCacheService = memoryCacheService;
            _validator = validator;
            _logger = logger;
            _boardSettings = boardSettings.Value;
            _mapper = mapper;
        }

        public async Task<Guid> CreateBoardAsync(BoardInputDTO boardInputDTO)
        {
            await AssertValidBoardInputDTOAsync(boardInputDTO);

            var aliveCells = _mapper.Map<IEnumerable<CellLocation>>(boardInputDTO.AliveCells);

            var board = new Board
            {
                Id = Guid.NewGuid(),
                Status = BoardStatus.Active,
                Rows = boardInputDTO.Rows,
                Columns = boardInputDTO.Columns,
                AliveCellsJson = JsonSerializer.Serialize(aliveCells),
                CreationDatetime = DateTime.UtcNow,
                LastUpdateDatetime = DateTime.UtcNow,
                CurrentGeneration = 0
            };

            await _unitOfWork.BoardRepository.InsertAsync(board);
            await _unitOfWork.SaveChangesAsync();

            _memoryCacheService.AddToCache(board.Id.ToString(), board);

            _logger.LogInformation("Board with Id {board.Id} created", board.Id);

            return board.Id;
        }

        public async Task<BoardOutputDTO> GetBoardCurrentStatusAsync(Guid boardId)
        {
            var board = await GetBoardAsync(boardId);
            var result = _mapper.Map<BoardOutputDTO>(board);
            result.AliveCells = _mapper.Map<IEnumerable<CellLocationDTO>>(board.GetCells().GetAliveCells());

            return result;
        }

        public async Task<BoardOutputDTO> GetBoardNextGenerationAsync(Guid boardId)
        {
            var result = await ProcessBoardAsync(boardId, 1);

            return result;
        }

        public async Task<BoardOutputDTO> GetBoardNextNumberOfGenerationsAsync(Guid boardId, int NumberOfGenerations)
        {
            var result = await ProcessBoardAsync(boardId, NumberOfGenerations);

            return result;
        }

        public async Task<BoardOutputDTO> GetBoardUntilFinalizedAsync(Guid boardId)
        {
            var result = await ProcessBoardAsync(boardId, -1);

            return result;
        }

        private async Task<BoardOutputDTO> ProcessBoardAsync(Guid boardId, int numberOfGenerations)
        {
            var board = await GetBoardAsync(boardId);
            var cells = board.GetCells();

            if (board.Status != BoardStatus.Active)
                throw new BoardNotActiveException(boardId);

            // if numberOfGenerations is negative, it will loop until the board is finalized.
            var remainingGenerations = numberOfGenerations < 0 ? int.MaxValue : numberOfGenerations;

            while (remainingGenerations > 0)
            {
                var newCells = CalculateNextGeneration(cells);

                if (newCells.AreEqual(cells))
                {
                    board.Status = BoardStatus.Finalized;
                    break;
                }

                cells = newCells;
                board.CurrentGeneration++;

                if (numberOfGenerations >= 0)
                {
                    remainingGenerations--;
                }

                if (board.CurrentGeneration >= _boardSettings.MaxGeneration)
                {
                    board.Status = BoardStatus.Invalid;
                    break;
                }
            }

            var aliveCells = cells.GetAliveCells();

            board.LastUpdateDatetime = DateTime.UtcNow;
            board.AliveCellsJson = JsonSerializer.Serialize(aliveCells);

            _unitOfWork.BoardRepository.Update(board);
            await _unitOfWork.SaveChangesAsync();

            _memoryCacheService.AddToCache(board.Id.ToString(), board);

            if (board.Status == BoardStatus.Invalid)
            {
                throw new OverflowException($"Board with Id {board.Id} has exceeded maximum generation.");
            }

            var result = _mapper.Map<BoardOutputDTO>(board);
            result.AliveCells = _mapper.Map<IEnumerable<CellLocationDTO>>(aliveCells);

            return result;
        }

        private async Task<Board> GetBoardAsync(Guid boardId)
        {
            var board = _memoryCacheService.GetFromCache<Board>(boardId.ToString());
            board ??= await _unitOfWork.BoardRepository.GetByIdAsync(boardId);

            if (board == null)
            {
                throw new BoardNotFoundException(boardId);
            }

            return board;
        }

        private static byte[,] CalculateNextGeneration(byte[,] cells)
        {
            var rows = cells.GetLength(0);
            var cols = cells.GetLength(1);
            var newCells = new byte[rows, cols];

            // Anonymous function to get the number of alive neighbors for each cell.
            var getAliveNeighbors = new Func<CellLocationDTO, int>(cell =>
            {
                var aliveNeighbors = 0;

                for (int x = cell.X - 1; x <= cell.X + 1; x++)
                {
                    for (int y = cell.Y - 1; y <= cell.Y + 1; y++)
                    {
                        if (x >= 0 && x < rows && y >= 0 && y < cols && !(x == cell.X && y == cell.Y) && cells[x, y] == 1)
                        {
                            aliveNeighbors++;
                        }
                    }
                }

                return aliveNeighbors;
            });

            // Anonymous function to calculate the next status of each cell.
            var calculateNextStatus = new Func<CellLocationDTO, byte>(cell =>
            {
                var aliveNeighbors = getAliveNeighbors(cell);

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

        private static IEnumerable<CellLocationDTO> GetAllCells(int rows, int cols)
        {
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    yield return new CellLocationDTO(x, y);
                }
            }
        }

        private async Task AssertValidBoardInputDTOAsync(BoardInputDTO boardInputDTO)
        {
            var validationResult = await _validator.ValidateAsync(boardInputDTO);

            if (!validationResult.IsValid)
            {
                _logger.LogError(message: "Validation of BoardInputDTO failed");
                validationResult.Errors.ForEach(error =>
                {
                    _logger.LogError("Error occurred: {errorMessage}", error.ToString());
                });

                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}