using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddLoggingExtension();
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();
app.ConfigureApp();
app.Run();

