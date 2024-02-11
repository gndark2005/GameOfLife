using GameOfLife.DTO.Boards;
using GameOfLife.Services.Boards.Abstractions;
using Microsoft.AspNetCore.Mvc;

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

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpPost("CreateBoard")]
        public async Task<IActionResult> CreateBoardAsync(BoardInputDTO board)
        {
            var result = await _boardService.CreateBoardAsync(board);
            return Created(nameof(CreateBoardAsync), result);
        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoardOutputDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpGet("GetBoardCurrentStatusAsync")]
        public async Task<IActionResult> GetBoardCurrentStatusAsync([FromQuery] Guid boardId)
        {
            var result = await _boardService.GetBoardCurrentStatusAsync(boardId);
            return Ok(result);
        }


        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoardOutputDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpGet("GetBoardNextGeneration")]
        public async Task<IActionResult> GetBoardNextGenerationAsync([FromQuery] Guid boardId)
        {
            var result = await _boardService.GetBoardNextGenerationAsync(boardId);
            return Ok(result);
        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoardOutputDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpGet("GetBoardNextNumberOfGenerations")]
        public async Task<IActionResult> GetBoardNextNumberOfGenerationsAsync([FromQuery] Guid boardId, [FromQuery] int NumberOfGenerations)
        {
            var result = await _boardService.GetBoardNextNumberOfGenerationsAsync(boardId, NumberOfGenerations);
            return Ok(result);
        }

        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BoardOutputDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "GameOfLife-v1")]
        [HttpGet("GetBoardUntilFinalized")]
        public async Task<IActionResult> GetBoardUntilFinalizedAsync([FromQuery] Guid boardId)
        {
            var result = await _boardService.GetBoardUntilFinalizedAsync(boardId);
            return Ok(result);
        }
    }
}
