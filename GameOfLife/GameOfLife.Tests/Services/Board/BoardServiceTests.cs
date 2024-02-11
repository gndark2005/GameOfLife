using AutoMapper;
using FluentAssertions;
using FluentValidation;
using GameOfLife.Data.Repositories.Abstractions;
using GameOfLife.DTO.Boards;
using GameOfLife.Services.Boards.Abstractions;
using GameOfLife.Services.Boards.Configuration;
using GameOfLife.Services.Boards.Mapping;
using GameOfLife.Services.Boards.Services;
using GameOfLife.Services.Boards.Validators;
using GameOfLife.Services.Cache.Abstractions;
using GameOfLife.Tests.Builders.Board;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Json;
using Models = GameOfLife.Data.Models;

namespace GameOfLife.Tests.Services.Board
{

    public class BoardServiceTests
    {
        private readonly IValidator<BoardInputDTO> _validator;
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock;
        private readonly Mock<ILogger<BoardService>> _loggerMock;
        private readonly Mock<IOptions<BoardSettings>> _boardSettingsMock;
        private readonly IBoardService _boardService;

        public BoardServiceTests()
        {

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
            _loggerMock = new Mock<ILogger<BoardService>>();
            _boardSettingsMock = new Mock<IOptions<BoardSettings>>();

            var boardSettings = new BoardSettings()
            {
                MaxGeneration = 10
            };

            _boardSettingsMock.Setup(config => config.Value).Returns(boardSettings);
            _mapper = new Mapper(new MapperConfiguration(cfg =>
                        {
                            cfg.AddProfile(new BoardMapper());
                        }));


            _validator = new BoardInputDTOValidator();

            _boardService = new BoardService(
                _unitOfWorkMock.Object,
                _memoryCacheServiceMock.Object,
                _validator,
                _loggerMock.Object,
                _boardSettingsMock.Object,
                _mapper
                );
        }

        [Fact]
        public async Task CreateBoardAsync_GivenInvalidBoard_ShoulThrowValidationException()
        {
            // Arrange

            var boardInputDTO = new BoardInputDTOBuilder()
                .WithRows(0)
                .WithColumns(0)
                .Build();

            // Act & Assert

            await Assert.ThrowsAsync<ValidationException>(async () => await _boardService.CreateBoardAsync(boardInputDTO));
        }

        [Fact]
        public async Task GetBoardCurrentStatusAsync_GivenBoardID_ShouldReturnBoardNextGeneration()
        {
            // Arrange

            var boardAliveCells = new List<Models.CellLocation>()
                {
                   new (1,0),
                   new (1,1),
                   new (1,2)
                };

            var expectedAliveCells = new List<CellLocationDTO>()
                {
                   new (0,1),
                   new (1,1),
                   new (2,1)
                };

            var boardId = Guid.NewGuid();
            var boardDb = new BoardBuilder()
                .WithId(boardId)
                .WithCurrentGeneration(0)
                .WithAliveCellsJson(JsonSerializer.Serialize(boardAliveCells))
                .Build();

            _memoryCacheServiceMock
                .Setup(x => x.GetFromCache<Models.Board>(boardId.ToString()))
                .Returns(boardDb);
            _memoryCacheServiceMock
                .Setup(x => x.AddToCache(It.IsAny<string>(), It.IsAny<Models.Board>()))
                .Verifiable();


            _unitOfWorkMock.Setup(u => u.BoardRepository.Update(It.IsAny<Models.Board>())).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Verifiable();

            // Act

            var result = await _boardService.GetBoardNextGenerationAsync(boardId);

            // Assert

            result.CurrentGeneration.Should().Be(1);
            Assert.True(expectedAliveCells.SequenceEqual(result.AliveCells));
        }

        [Fact]
        public async Task GetBoardUntilFinalizedAsync_GivenBoardID_ShouldThrowOverflowException()
        {
            // Arrange

            var boardAliveCells = new List<Models.CellLocation>()
                {
                   new (1,0),
                   new (1,1),
                   new (1,2)
                };

            var expectedAliveCells = new List<CellLocationDTO>()
                {
                   new (0,1),
                   new (1,1),
                   new (2,1)
                };

            var boardId = Guid.NewGuid();
            var boardDb = new BoardBuilder()
                .WithId(boardId)
                .WithCurrentGeneration(0)
                .WithAliveCellsJson(JsonSerializer.Serialize(boardAliveCells))
                .Build();

            _memoryCacheServiceMock
                .Setup(x => x.GetFromCache<Models.Board>(boardId.ToString()))
                .Returns(boardDb);
            _memoryCacheServiceMock
                .Setup(x => x.AddToCache(It.IsAny<string>(), It.IsAny<Models.Board>()))
                .Verifiable();


            _unitOfWorkMock.Setup(u => u.BoardRepository.Update(It.IsAny<Models.Board>())).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Verifiable();

            // Act & Assert

            await Assert.ThrowsAsync<OverflowException>(async () => await _boardService.GetBoardUntilFinalizedAsync(boardId));
        }
    }
}
