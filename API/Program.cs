using APPLICATION;
using INFRASTRUCTURE;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // 
}

app.UseHttpsRedirection();

app.Run();