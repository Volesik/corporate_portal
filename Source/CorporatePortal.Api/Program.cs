using CorporatePortal.Api;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.BL.Services;
using CorporatePortal.DL;
using CorporatePortal.DL.EntityFramework;
using CorporatePortal.DL.Repositories;
using CorporatePortal.Web.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CorporatePortalContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddWebCommonServices(builder.Configuration);
builder.Services.AddScoped(typeof(IDatabaseContextRepository<>), typeof(DatabaseContextRepository<>));
builder.Services.AddScoped<IDatabaseContextRepository, DatabaseContextRepository>();
builder.Services.AddTransient<IStartupFilter, MigrationStartupFilter<CorporatePortalContext>>();
builder.Services.AddTransient<IUserInfoService, UserInfoService>();
builder.Services.AddTransient<IExternalUserDataService, ExternalUserDataService>();
builder.Services.AddTransient<IUserInfoListSearchService, UserInfoListSearchService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
