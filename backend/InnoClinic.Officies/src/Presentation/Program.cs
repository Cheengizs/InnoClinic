using System.Data;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

app.MapGet("api/v1/officies", () =>
{
    // validation
    
    // use case call
    
    // result sending
    
});

public class SomeRequest
{
    public string Name { get; set; }
    public string Address { get; set; }
}
