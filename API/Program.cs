using APPLICATION;
using APPLICATION.Interfaces.Services;
using APPLICATION.Services;
using INFRASTRUCTURE;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register Decorator (Standard):
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserService>(provider =>
{
    var userService = provider.GetRequiredService<UserService>();
    var cacheService = provider.GetRequiredService<ICacheService>();

    return new CachedUserService(userService, cacheService);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.Run();