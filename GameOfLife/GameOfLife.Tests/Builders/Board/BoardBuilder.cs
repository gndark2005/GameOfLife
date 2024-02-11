namespace GameOfLife.Tests.Builders.Board
{
    using GameOfLife.Data.Models;
    using System;

    public class BoardBuilder
    {
        private Guid _id;
        private BoardStatus _status;
        private int _rows;
        private int _columns;
        private string _aliveCellsJson;
        private DateTime _creationDatetime;
        private DateTime _lastUpdateDatetime;
        private int _currentGeneration;

        public BoardBuilder()
        {
            _id = Guid.NewGuid();
            _status = BoardStatus.Active;
            _rows = 3;
            _columns = 3;
            _aliveCellsJson = "\"[{\"\"X\"\": 1, \"\"Y\"\": 0}, {\"\"X\"\": 1, \"\"Y\"\": 1}, {\"\"X\"\": 1, \"\"Y\"\": 2}]\"";
            _creationDatetime = DateTime.UtcNow;
            _lastUpdateDatetime = DateTime.UtcNow;
            _currentGeneration = 0;
        }

        public BoardBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public BoardBuilder WithStatus(BoardStatus status)
        {
            _status = status;
            return this;
        }

        public BoardBuilder WithRows(int rows)
        {
            _rows = rows;
            return this;
        }

        public BoardBuilder WithColumns(int columns)
        {
            _columns = columns;
            return this;
        }

        public BoardBuilder WithAliveCellsJson(string aliveCellsJson)
        {
            _aliveCellsJson = aliveCellsJson;
            return this;
        }

        public BoardBuilder WithCreationDatetime(DateTime creationDatetime)
        {
            _creationDatetime = creationDatetime;
            return this;
        }

        public BoardBuilder WithLastUpdateDatetime(DateTime lastUpdateDatetime)
        {
            _lastUpdateDatetime = lastUpdateDatetime;
            return this;
        }

        public BoardBuilder WithCurrentGeneration(int currentGeneration)
        {
            _currentGeneration = currentGeneration;
            return this;
        }

        public Board Build()
        {
            return new Board
            {
                Id = _id,
                Status = _status,
                Rows = _rows,
                Columns = _columns,
                AliveCellsJson = _aliveCellsJson,
                CreationDatetime = _creationDatetime,
                LastUpdateDatetime = _lastUpdateDatetime,
                CurrentGeneration = _currentGeneration
            };
        }
    }
}
