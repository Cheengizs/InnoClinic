using Presentation.DiConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddLogger();
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.AddMiddlewares();

app.Run();
