using LojaVirtual.Api.Configurations;
using LojaVirtual.Core.Infra.Seed;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApiConfig()
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
