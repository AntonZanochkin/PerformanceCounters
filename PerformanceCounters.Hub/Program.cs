using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PerformanceCounters.Hub.EF;
using PerformanceCounters.Hub.EF.Interceptors;
using PerformanceCounters.Hub.Logs;
using PerformanceCounters.Hub.Services;
using PerformanceCounters.Hub.Services.SignalR;
using PerformanceCounters.Hub.SignalR.Hubs;

namespace PerformanceCounters.Hub
{
  public class Program
  {
    public const string CustomCookieScheme = nameof(CustomCookieScheme);
    public const string CustomTokenScheme = nameof(CustomTokenScheme);

    public static void Main(string[] args)
    {
      DotNetEnv.Env.Load();

      var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllers();

      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      builder.Services.AddScoped<DbCacheService>();

      builder.Services.AddScoped<DeviceService>();
      builder.Services.AddScoped<ProcessService>();
      builder.Services.AddScoped<CounterService>();

      builder.Services.AddScoped<DeviceSignalService>();
      builder.Services.AddScoped<ProcessSignalService>();
      builder.Services.AddScoped<CounterSignalService>();

      builder.Services.AddSignalR().AddJsonProtocol(options =>
      {
        options.PayloadSerializerOptions.Converters
          .Add(new JsonStringEnumConverter());
      }); 

      builder.Services.AddDbContext<CountersDbContext>(options =>
      {
        var loggerFactory = new LoggerFactory();
        loggerFactory.AddProvider(new EfLoggerProvider());

        var connectionString = Environment.GetEnvironmentVariable("DOCKER_CONNECTION_STRING") ?? Environment.GetEnvironmentVariable("CONNECTION_STRING");
        Console.WriteLine($"CONNECTION_STRING:{connectionString}");
        options.UseSqlServer(connectionString)
          .UseLoggerFactory(loggerFactory)
          .AddInterceptors(new EfInterceptor());
      });

      builder.Services.AddCors(options =>
      {
        options.AddDefaultPolicy(builder =>
        {
          builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
      });

      builder.Services.AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          // JWT configuration...
        });

      var app = builder.Build();

      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors(x => x
          .AllowAnyMethod()
          .AllowAnyHeader()
          .SetIsOriginAllowed(origin => true) 
          .AllowCredentials());
      }

      app.UseCors();
      app.UseAuthorization();

      app.MapControllers();
      app.MapHub<ClientHub>("/hub/client");

      app.Run();
    }
  }
}