using AutoMapper;
using GameOfLife.Data.Models;
using GameOfLife.DTO.Boards;

namespace GameOfLife.Services.Boards.Mapping
{
    public  class BoardMapper : Profile
    {
        public BoardMapper()
        {
            CreateMap<BoardStatus,BoardStatusDTO>().ReverseMap();

            CreateMap<CellLocation, CellLocationDTO>().ReverseMap();

            CreateMap<Board, BoardOutputDTO>()
                .ForMember(dest => dest.BoardId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
