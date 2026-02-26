using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Bulgarikon.Data;

namespace Bulgarikon.Tests.Infrastructure
{
    public static class TestDbFactory
    {
        public static BulgarikonDbContext CreateInMemory()
        {
            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseInMemoryDatabase("BulgarikonDb_" + Guid.NewGuid())
                .EnableSensitiveDataLogging()
                .Options;

            return new BulgarikonDbContext(options);
        }

        // For ExecuteUpdateAsync / ExecuteDeleteAsync (FeedbackService)
        public static (BulgarikonDbContext db, SqliteConnection conn) CreateSqliteInMemory()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            conn.Open();

            var options = new DbContextOptionsBuilder<BulgarikonDbContext>()
                .UseSqlite(conn)
                .EnableSensitiveDataLogging()
                .Options;

            var db = new BulgarikonDbContext(options);
            db.Database.EnsureCreated();
            return (db, conn);
        }
    }
}