using Microsoft.EntityFrameworkCore;

namespace ConsistentHashing
{
    public class ShardDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;

        private readonly string _connectionString;

        public ShardDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
