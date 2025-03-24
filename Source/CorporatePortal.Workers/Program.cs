using CorporatePortal.BL.Extensions;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.BL.Services;
using Hangfire;
using Hangfire.PostgreSql;
using CorporatePortal.Common.Constants;
using CorporatePortal.DL;
using CorporatePortal.DL.EntityFramework;
using CorporatePortal.DL.Repositories;
using CorporatePortal.Workers.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Hangfire configuration
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
builder.Services.AddTransient<IDownloader, Downloader>();
builder.Services.AddTransient<IUserPhotoService, UserPhotoService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHangfireDashboard();

// Configure the HTTP request pipeline.
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