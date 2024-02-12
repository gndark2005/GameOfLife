using GameOfLife.Api.Configurations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddBusinessServices(builder.Configuration);

var app = builder.Build();

app.ConfigureSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseExceptionHandlerMiddleware();
app.MapControllers();
app.Run();
