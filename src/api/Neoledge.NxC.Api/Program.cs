using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Minio;
using Neoledge.NxC.Api.Extensions.DependencyInjection;
using Neoledge.NxC.Api.Middlewares;
using Neoledge.NxC.Database;
using Neoledge.NxC.Database.Extensions.DependencyInjection;
using Neoledge.NxC.Repository.Extensions.DependencyInjection;
using Neoledge.NxC.Service.NxC.Extensions.DependencyInjection;
using Neoledge.NxC.Service.NxC.Extensions.Options;
using Scalar.AspNetCore;
using MinioConfig = Neoledge.NxC.Service.NxC.Extensions.Options.MinioConfig;

var builder = WebApplication.CreateBuilder(args);
//added for env vars 
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.Configure<KeycloakOptions>(builder.Configuration.GetSection("Keycloak"));
builder.Services.Configure<MinioConfig>(builder.Configuration.GetSection("Minio"));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepository();

builder.Services.AddNxCServices(builder.Configuration);

var version = new List<ApiVersion> { new ApiVersion(1), new ApiVersion(2) };
builder.Services.AddVersioning()
                .AddScalar(version)
                .AddKeycloak(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapOpenApi();

app.MapScalarApiReference((options, context) =>
{
    options
        .WithTheme(ScalarTheme.Solarized)
        .WithLayout(ScalarLayout.Modern)
        .WithFavicon("https://scalar.com/logo-light.svg")
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

    options.AddPreferredSecuritySchemes(["Bearer"]);

    options.AddImplicitFlow("Bearer", flow =>
    {
        flow.ClientId = "nxc";
        flow.AuthorizationUrl = "http://localhost:8080/realms/NxC/protocol/openid-connect/auth";
    })
    .AddDefaultScopes("Bearer", ["openid", "profile"]);
});

app.MapControllers();
app.UseMiddleware<ApiExceptionMiddleware>();
await app.RunAsync();