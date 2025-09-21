using fit_flow_users.WebApi.Services;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.System.Text.Json;
using System.Configuration;
using Sentry;
using Sentry.AspNetCore;

try
{
    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<RedisService>();
    builder.Services.AddScoped<UserService>();

    //Routines service configuration
    builder.Services.AddHttpClient<GoalService>(client => client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ROUTINES_URL")))
        .AddResilienceHandler("GoalServiceHandler", pipeline =>
        {
            int defaultRetryAttemps = builder.Configuration.GetValue<int>("DefaultRetryAttemps");

            //General reques timeout
            pipeline.AddTimeout(TimeSpan.FromSeconds(5));

            //Retry with Exponential Backoff and Jitter
            pipeline.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = defaultRetryAttemps,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromMicroseconds(500)
            });

            //Timeout per try
            pipeline.AddTimeout(TimeSpan.FromSeconds(5));

            //Circuit Breaker
            pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration = TimeSpan.FromSeconds(10),
                FailureRatio = 0.9,
                MinimumThroughput = 10,
                BreakDuration = TimeSpan.FromSeconds(5)
            });
        });

    // Add Redis Database as a singleton
    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
        if (configuration == null)
            throw new Exception("Not able to find Redis connection string in configuration file");
        
        return ConnectionMultiplexer.Connect(configuration);
    });

    //Defailt Timeout
    builder.Services.AddRequestTimeouts(options =>
    {
        int defaultTimeout = builder.Configuration.GetValue<int>("DefaultTimeoutInSeconds");
        options.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromSeconds(defaultTimeout) };
    });
    
    // Add Sentry
    builder.WebHost.UseSentry(options =>
    {
        options.Dsn = Environment.GetEnvironmentVariable("SENTRY_DSN");
        options.Debug = true;
        options.TracesSampleRate = 1;
        options.SendDefaultPii = true;
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
    SentrySdk.CaptureException(ex);
}
