namespace ConsistentHashing
{
    public class DatabaseInitializer
    {
        private readonly List<string> _shardConnectionStrings;

        public DatabaseInitializer()
        {
            _shardConnectionStrings = new List<string>
            {
                 "Server=localhost\\SQLEXPRESS;Database=Shard2;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;",
                 "Server=localhost\\SQLEXPRESS;Database=Shard3;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;",
                 "Server=localhost\\SQLEXPRESS;Database=Shard1;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;",
                 "Server=localhost\\SQLEXPRESS;Database=Shard4;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;",
            };
        }

        public void EnsureDatabasesCreated()
        {
            foreach (var connStr in _shardConnectionStrings)
            {
                using var context = new ShardDbContext(connStr);
                context.Database.EnsureCreated();
                Console.WriteLine($"Initialized: {connStr}");
            }
        }
    }

}
