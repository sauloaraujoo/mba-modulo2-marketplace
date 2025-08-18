using LojaVirtual.Data.Seed;
using LojaVirtual.Mvc.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDbContextIdentityConfig(builder.Configuration)
    .RegisterServices()
    .AddAutoMapper()
    .AddMvcConfiguration();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/erro/500");
    app.UseHsts();
}
app.UseStatusCodePagesWithRedirects("/erro/{0}");


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseGlobalizationConfig();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//app.UseDbMigrationHelper();

app.Run();
