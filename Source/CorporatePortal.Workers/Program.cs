using CorporatePortal.BL.Interfaces;
using CorporatePortal.BL.Services;
using Hangfire;
using Hangfire.PostgreSql;
using CorporatePortal.Common.Constants;
using CorporatePortal.DL;
using CorporatePortal.DL.EntityFramework;
using CorporatePortal.DL.Repositories;
using CorporatePortal.Web.Common.Extensions;
using CorporatePortal.Workers.Extensions;
using CorporatePortal.Workers.Mappers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var hangfireQueues = new List<string>();
builder.Configuration.GetSection("Hangfire:Queues").Bind(hangfireQueues);

if (hangfireQueues.Count == 0)
{
    hangfireQueues = WorkerQueueConstants.QueuesForRegistration;
}

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(options => options
        .UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddHangfireServer(options =>
{
    options.Queues = hangfireQueues.ToArray();
});

builder.Services.AddDbContext<CorporatePortalContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IDatabaseContextRepository<>), typeof(DatabaseContextRepository<>));
builder.Services.AddScoped<IDatabaseContextRepository, DatabaseContextRepository>();
builder.Services.AddWebCommonServices(builder.Configuration);
builder.Services.AddTransient<IUserInfoService, UserInfoService>();
builder.Services.AddSingleton<IExternalUserDataService, ExternalUserDataService>();
builder.Services.AddTransient<UserInfoMapper>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHangfireDashboard();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHangfireDashboard();
app.RegisterRecurringJobs();
app.Run();