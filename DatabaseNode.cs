
namespace ConsistentHashing
{
    public class DatabaseNode
    {
        public string Name { get; }
        public string ConnectionString { get; }

        public DatabaseNode(string name, string connectionString)
        {
            Name = name;
            ConnectionString = connectionString;
        }

        public void InsertUser(string name)
        {
            using var context = new ShardDbContext(ConnectionString);
            context.Users.Add(new User { Name = name });
            context.SaveChanges();
            Console.WriteLine($"Inserted User {Name}");
        }

        public List<User> GetUsers()
        {
            using var context = new ShardDbContext(ConnectionString);
            return context.Users.ToList();
        }

        public string? GetUser(string name)
        {
            using var context = new ShardDbContext(ConnectionString);
            var user = context.Users.Where(u => u.Name.Equals(name)).FirstOrDefault();
            return user?.Name;
        }

        public override string ToString() => Name;

        public void DeleteUser(User user)
        {
            using var context = new ShardDbContext(ConnectionString);
            context.Users.Remove(user);
            context.SaveChanges();
        }
    }


}
