

using Neoledge.NxC.Database;
using Neoledge.NxC.DatabaseMigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<AppDbContext>("DefaultConnection");

var host = builder.Build();
await host.RunAsync();