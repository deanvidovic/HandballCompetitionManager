using HandballCompetitionManager.Interfaces;
using HandballCompetitionManager.Services.Mock;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITournamentRepository, MockTournamentRepository>();
builder.Services.AddScoped<ITeamRepository, MockTeamRepository>();
builder.Services.AddScoped<IPlayerRepository, MockPlayerRepository>();
builder.Services.AddScoped<IMatchRepository, MockMatchRepository>();
builder.Services.AddScoped<IMatchEventRepository, MockMatchEventRepository>();
builder.Services.AddScoped<IUserRepository, MockUserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
