using GameOfLife.DTO.Boards;
using GameOfLife.Services.Boards.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameOfLife.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : Controller
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        /// <summary>
        /// Creates a new game board.
        /// </summary>
        /// <param name="board">The input data for creating the board.</param>
        /// <returns>The created board's ID.</returns>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpPost("")]
        public async Task<IActionResult> CreateBoardAsync(BoardInputDTO board)
        {
            var result = await _boardService.CreateBoardAsync(board);
            return Created(nameof(CreateBoardAsync), result);
        }

        /// <summary>
        /// Retrieves the current status of a game board.
        /// </summary>
        /// <param name="boardId">The ID of the board to retrieve status for.</param>
        /// <returns>The current status of the board.</returns>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoardOutputDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpGet("{boardId}")]
        public async Task<IActionResult> GetBoardCurrentStatusAsync([FromRoute] Guid boardId)
        {
            var result = await _boardService.GetBoardCurrentStatusAsync(boardId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the next generation of a game board.
        /// </summary>
        /// <param name="boardId">The ID of the board to retrieve the next generation for.</param>
        /// <returns>The next generation of the board.</returns>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoardOutputDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpGet("{boardId}/GetBoardNextGeneration")]
        public async Task<IActionResult> GetBoardNextGenerationAsync([FromRoute] Guid boardId)
        {
            var result = await _boardService.GetBoardNextGenerationAsync(boardId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the board after running a specified number of generations.
        /// </summary>
        /// <param name="boardId">The ID of the board to retrieve generations for.</param>
        /// <param name="NumberOfGenerations">The number of generations to run.</param>
        /// <returns>The board after running the specified number of generations.</returns>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoardOutputDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpGet("{boardId}/GetBoardNextNumberOfGenerations/{NumberOfGenerations}")]
        public async Task<IActionResult> GetBoardNextNumberOfGenerationsAsync([FromRoute] Guid boardId, [FromRoute][Range(1, int.MaxValue)] int NumberOfGenerations)
        {
            var result = await _boardService.GetBoardNextNumberOfGenerationsAsync(boardId, NumberOfGenerations);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the board until it reaches a final state.
        /// </summary>
        /// <param name="boardId">The ID of the board to retrieve generations for.</param>
        /// <returns>The board until it reaches a final state.</returns>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoardOutputDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpGet("{boardId}/GetBoardUntilFinalized")]
        public async Task<IActionResult> GetBoardUntilFinalizedAsync([FromRoute] Guid boardId)
        {
            var result = await _boardService.GetBoardUntilFinalizedAsync(boardId);
            return Ok(result);
        }
    }
}
