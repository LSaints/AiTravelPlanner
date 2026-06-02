using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ATP.Web.Components;
using ATP.Web.Features.Trips.Application;
using ATP.Web.Features.TravelPlans.Application;
using ATP.Web.Infrastructure.Persistence;
using ATP.Web.Infrastructure.Persistence.Repositories;
using ATP.Web.Infrastructure.AI;
using ATP.Web.Shared.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<ITravelPlanRepository, TravelPlanRepository>();

builder.Services.AddScoped<CreateTrip>();
builder.Services.AddScoped<UpdateTrip>();
builder.Services.AddScoped<GetTrip>();
builder.Services.AddScoped<DeleteTrip>();
builder.Services.AddScoped<GenerateTravelPlan>();
builder.Services.AddScoped<RegenerateTravelPlan>();

builder.Services.Configure<OpenAiOptions>(builder.Configuration.GetSection(OpenAiOptions.SectionName));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<OpenAiOptions>>().Value);
builder.Services.AddHttpClient<IAIProvider, OpenAiProvider>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
