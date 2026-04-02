using APPLICATION;
using APPLICATION.Interfaces.Services;
using APPLICATION.Services;
using INFRASTRUCTURE;
using INFRASTRUCTURE.Caching;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
builder.Services.AddScoped<ICacheService, RedisCacheService>();

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