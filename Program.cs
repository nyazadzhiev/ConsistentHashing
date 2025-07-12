using ConsistentHashing;

public static class Program
{
    public static void Main()
    {
        var dbInit = new DatabaseInitializer();
        dbInit.EnsureDatabasesCreated();

        Console.WriteLine("All shards initialized.");

        var shard1 = new DatabaseNode("Shard1", "Server=localhost\\SQLEXPRESS;Database=Shard1;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
        var shard2 = new DatabaseNode("Shard2", "Server=localhost\\SQLEXPRESS;Database=Shard2;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
        var shard3 = new DatabaseNode("Shard3", "Server=localhost\\SQLEXPRESS;Database=Shard3;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
        var shard4 = new DatabaseNode("Shard4", "Server=localhost\\SQLEXPRESS;Database=Shard4;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");

        var hashRing = new ConsistentHash<DatabaseNode>(virtualNodes: 100);
        hashRing.AddNode(shard1);
        hashRing.AddNode(shard2);
        hashRing.AddNode(shard3);
        hashRing.AddNode(shard4);

        var users = new[]
        {
            "Alice",
            "Bob",
            "Charlie",
            "Diana"
        };


        // INSERT
        foreach (var user in users)
        {
            var node = hashRing.GetNode(user);
            node.InsertUser(user);
        }

        // READ
        Console.WriteLine("Reading users from correct shard:");
        foreach (var user in users)
        {
            var node = hashRing.GetNode(user);
            var name = node.GetUser(user);
            Console.WriteLine($"User ({name}) is in {node}");
        }

        var usersFromShard3 = shard3.GetUsers();

        hashRing.RemoveNode(shard3);

        foreach (var user in usersFromShard3)
        {
            var newNode = hashRing.GetNode(user.Name);
            newNode.InsertUser(user.Name);   
            shard3.DeleteUser(user);        
        }

        // READ
        Console.WriteLine("Reading users from correct shard:");
        foreach (var user in users)
        {
            var node = hashRing.GetNode(user);
            var name = node.GetUser(user);
            Console.WriteLine($"User ({name}) is in {node}");
        }
    }
}
