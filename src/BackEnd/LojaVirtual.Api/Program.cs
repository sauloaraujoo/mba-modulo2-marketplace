using LojaVirtual.Api.Configurations;
using LojaVirtual.Data.Seed;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApiConfig()
    .AddApiVersioningConfig()
    .AddCorsConfig(builder.Configuration)
    .AddSwaggerConfiguration()
    .AddDbContextConfig(builder.Configuration)
    .AddIdentityConfig()
    .RegisterServices()
    .AddJwtConfig(builder.Configuration)
    .AddAutoMapper();

var app = builder.Build();

app.UseSwaggerConfiguration()
   .UseApiConfiguration(app.Environment);

app.UseDbMigrationHelper();

app.Run();
