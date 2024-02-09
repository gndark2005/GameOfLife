using FluentValidation;
using GameOfLife.Data.Data;
using GameOfLife.Data.Data.Abstractions;
using GameOfLife.Data.Repositories;
using GameOfLife.Data.Repositories.Abstractions;
using GameOfLife.DTO.Boards;
using GameOfLife.Services.Cache.Abstractions;
using GameOfLife.Services.Cache.Services;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Api.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            return services

                // Data
                .AddDbContext<GameOfLifeDBContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))

                .AddScoped<IGameOfLifeDBContext, GameOfLifeDBContext>()
                .AddScoped<IUnitOfWork, UnitOfWork>()

                // Cache
                .AddMemoryCache()
                .AddScoped<IMemoryCacheService, MemoryCacheService>()

                // Validators
                .AddValidatorsFromAssemblyContaining<BoardInputDTO>();
        }
    }
}
