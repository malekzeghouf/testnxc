using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.AppHost
{
    public static class SqlMigrationRunner
    {
        public static async Task ApplyMigrationsAsync(string connectionString, string scriptsFolder)
        {
            foreach (var file in Directory.GetFiles(scriptsFolder, "*.sql"))
            {
                var script = await File.ReadAllTextAsync(file);

                using var conn = new SqlConnection(connectionString);
                await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = script;
                await cmd.ExecuteNonQueryAsync();

                Console.WriteLine($"✅ Executed: {Path.GetFileName(file)}");
            }
        }
    }
}
