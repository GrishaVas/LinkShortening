using LinkShortening.Infrastructure;
using LinkShortening.MVC.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LinkShorteningDbContext>(opts =>
{
    var connectionString = builder.Configuration["MySql:ConnectionString"];

    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddRepositoriesConfiguration();
builder.Services.AddServicesConfiguration();

var app = builder.Build();

await MigrateDatabase(app);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Links}/{action=Index}/{id?}");

app.Run();

static async Task MigrateDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<LinkShorteningDbContext>();

    await dbContext.Database.MigrateAsync();
}