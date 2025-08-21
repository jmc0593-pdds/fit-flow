using fit_flow_users.WebApi.Services;
using StackExchange.Redis;

try
{
    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<RedisService>();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<GoalService>();
    // Add Redis as a singleton
    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var configuration = builder.Configuration.GetConnectionString("Redis");
        if (configuration == null)
            throw new Exception("Not able to find Redis connection string in configuration file");
        return ConnectionMultiplexer.Connect(configuration);
    });
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
