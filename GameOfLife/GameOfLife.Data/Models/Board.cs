﻿namespace GameOfLife.Data.Models
{
    public class Board : ModelBase
    {
        public override Guid Id { get; set; }

        public BoardStatus Status { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public string AliveCellsJson { get; set; }

        public DateTime CreationDatetime { get; set; }

        public DateTime LastUpdateDatetime { get; set; } 

        public int CurrentGeneration { get; set; }
    }
}
