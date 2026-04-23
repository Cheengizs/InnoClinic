using InnoClinic.Profiles.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencies();

var app = builder.Build();

app.UseDependencies();

app.AddControllers();

app.Run();
