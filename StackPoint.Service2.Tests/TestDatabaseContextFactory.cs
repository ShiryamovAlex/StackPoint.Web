using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StackPoint.Data;

namespace StackPoint.Service2.Tests
{
    public class TestDatabaseContextFactory : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<DatabaseContext> _options;

        public TestDatabaseContextFactory()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = new DatabaseContext(_options))
            {
                context.Database.EnsureCreated();
            }
        }

        public DatabaseContext GetTestDbContext()
        {
            return new DatabaseContext(_options);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
