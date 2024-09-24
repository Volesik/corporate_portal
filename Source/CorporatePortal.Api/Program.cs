using CorporatePortal.Api;
using CorporatePortal.BL.Interfaces;
using CorporatePortal.BL.Services;
using CorporatePortal.DL;
using CorporatePortal.DL.EntityFramework;
using CorporatePortal.DL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CorporatePortalContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IDatabaseContextRepository<>), typeof(DatabaseContextRepository<>));
builder.Services.AddScoped<IDatabaseContextRepository, DatabaseContextRepository>();
builder.Services.AddTransient<IStartupFilter, MigrationStartupFilter<CorporatePortalContext>>();
builder.Services.AddTransient<IUserInfoService, UserInfoService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

var fileProvider = new PhysicalFileProvider(
	Path.Combine(app.Environment.WebRootPath));
app.MapFallbackToFile("index.html", new StaticFileOptions {
	FileProvider = fileProvider,
	RequestPath = ""
});

app.Run();
