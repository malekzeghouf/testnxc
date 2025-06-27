using Microsoft.EntityFrameworkCore;
using Neoledge.NxC.Database;
using Neoledge.NxC.Database.Models;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Neoledge.NxC.DatabaseMigrationService;

public class Worker(IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<Worker> logger) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            logger.LogInformation("Starting database migration...");
            await RunMigrationAsync(dbContext, stoppingToken);
            logger.LogInformation("Database migration completed successfully.");

            logger.LogInformation("Seeding database...");
            //await SeedDataAsync(dbContext, stoppingToken);
            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        Federation fed = new()
        {
            Id = "gec",
            Name = "gec",
            Description = "gec",
            Enabled = true,
        };
        Member mehat = new()
        {
            Id = "mehat-gec",
            Name = "mehat-gec",
            Description = "mehat-gec",
            Enabled = true,
            FederationId = fed.Id
        };
        Member mtec = new()
        {
            Id = "mtec-gec",
            Name = "mtec-gec",
            Description = "mtec-gec",
            Enabled = true,
            FederationId = fed.Id
        };

        Inbox mehatInbox = new()
        {
            Id = "sdk-mehat-gec",
            Name = "sdk-mehat-gec",
            Description = "sdk-mehat-gec",
            Enabled = true,
            MemberId = mehat.Id,
            Member = mehat
        };

        Inbox mtecInbox = new()
        {
            Id = "drh-mtek-gec",
            Name = "drh-mtek-gec",
            Description = "drh-mtek-gec",
            Enabled = true,
            MemberId = mtec.Id,
            Member = mtec
        };
        //InboxPublicCertificate drh-mtek - gecpub = new()
        //{
        //    MemberId = mtec.Id,
        //    Active = true,
        //    PublicCertificatePem = "-----BEGIN CERTIFICATE-----\r\n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\r\ngeLqCH00G2umFFZKL78fJ9ig\r\n-----END CERTIFICATE-----"
        //};
        //InboxPublicCertificate mhatpub = new()
        //{
        //    MemberId = mehat.Id,
        //    Active = true,
        //    PublicCertificatePem = "-----BEGIN CERTIFICATE-----\r\n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\r\ngeLqCH00G2umFFZKL78fJ9ig\r\n-----END CERTIFICATE-----"
        //};

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Federations.AddAsync(fed, cancellationToken);
            await dbContext.Members.AddAsync(mtec, cancellationToken);
            await dbContext.Members.AddAsync(mehat, cancellationToken);
            await dbContext.Inboxes.AddAsync(mehatInbox, cancellationToken);
            await dbContext.Inboxes.AddAsync(mtecInbox, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}