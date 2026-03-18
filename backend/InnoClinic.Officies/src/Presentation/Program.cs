using Presentation.Extensions;
using Presentation.Middlewares;
using Presentation.MinimalApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.AddLoggingExtension();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<LoggingHandlerMiddleware>();
app.UseHttpsRedirection();

app.MapGroup("/api/v1/offices")
    .MapOffices();

app.Run();

