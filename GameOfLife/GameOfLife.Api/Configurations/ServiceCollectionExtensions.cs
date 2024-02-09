using GameOfLife.Data.Data;
using GameOfLife.Data.Data.Abstractions;
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

                .AddScoped<IGameOfLifeDBContext, GameOfLifeDBContext>();
        }
    }
}
