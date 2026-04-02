using APPLICATION;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // 
}

app.UseHttpsRedirection();

app.Run();