using FluentValidation;
using GameOfLife.DTO.Boards;

namespace GameOfLife.Services.Boards.Validators
{
    public class BoardInputDTOValidator : AbstractValidator<BoardInputDTO>
    {
        public BoardInputDTOValidator()
        {
            RuleFor(board => board.Rows)
                .GreaterThan(0)
                .WithMessage("Rows count must be greater than 0.");

            RuleFor(board => board.Cols)
                .GreaterThan(0)
                .WithMessage("Cols count must be greater than 0.");

            RuleFor(board => board.AliveCells)
                .NotNull().WithMessage("Alive cells count cannot be null.")
                .NotEmpty().WithMessage("Alive cells count cannot be empty.");

            RuleFor(board => board)
                .Must(IsValidAliveCells)
                .WithMessage("There are invalid alive cells coordinates.");
        }

        private bool IsValidAliveCells(BoardInputDTO board)
        {
            foreach (var cell in board.AliveCells)
            {
                if (cell.X < 0 || cell.X >= board.Rows || cell.Y < 0 || cell.Y >= board.Cols)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
