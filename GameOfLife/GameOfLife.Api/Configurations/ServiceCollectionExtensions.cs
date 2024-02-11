using FluentValidation;
using GameOfLife.Data.Data;
using GameOfLife.Data.Data.Abstractions;
using GameOfLife.Data.Repositories;
using GameOfLife.Data.Repositories.Abstractions;
using GameOfLife.Services.Boards.Abstractions;
using GameOfLife.Services.Boards.Configuration;
using GameOfLife.Services.Boards.Services;
using GameOfLife.Services.Boards.Validators;
using GameOfLife.Services.Cache.Abstractions;
using GameOfLife.Services.Cache.Configuration;
using GameOfLife.Services.Cache.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace GameOfLife.Api.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            return services

                // Data
                .AddDbContext<GameOfLifeDBContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("ApplicationDb")))

                .AddScoped<IGameOfLifeDBContext, GameOfLifeDBContext>()
                .AddScoped<IUnitOfWork, UnitOfWork>()

                // Cache
                .AddMemoryCache()
                .AddScoped<IMemoryCacheService, MemoryCacheService>()

                // Validators
                .AddValidatorsFromAssemblyContaining<BoardInputDTOValidator>()

                // Services
                .AddScoped<IBoardService, BoardService>()

                // Mappers
                .AddAutoMapper(typeof(Services.Boards.Mapping.BoardMapper))

                // Settings
                .Configure<BoardSettings>(configuration.GetSection("BoardSettings"))
                .Configure<MemoryCacheSettings>(configuration.GetSection("MemoryCacheSettings"))

                // Swagger
                .AddSwaggerGen(options =>
                     {
                         options.SwaggerDoc("GameOfLife-v1", new OpenApiInfo { Title = "Game Of Life API", Version = "v1" });
                     });
        }
    }
}
