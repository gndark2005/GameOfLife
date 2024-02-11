using GameOfLife.Api.Middlewares;

namespace GameOfLife.Api.Configurations
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/GameOfLife-v1/swagger.json", "Game Of Life API V1");
                    });
        }

        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
