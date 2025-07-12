# Consistent Hashing with Virtual Nodes - Database Sharding Example

This project demonstrates an implementation of consistent hashing with virtual nodes in C# to distribute data across multiple database shards efficiently.

## Overview

Consistent hashing is a popular technique used in distributed systems to distribute keys (e.g., user records) evenly across a dynamic set of nodes (e.g., database servers). This approach minimizes data movement when nodes are added or removed, improving scalability and fault tolerance.

This project implements:

- A generic consistent hashing ring with virtual nodes for balanced distribution.
- Dynamic addition and removal of database nodes.
- Automatic redirection of keys to appropriate shards.
- Data migration logic when removing nodes from the ring.
- Example usage with SQL Server shards via Entity Framework Core.

## Features

- **Virtual nodes:** Each physical node is represented by multiple virtual nodes to achieve better load balancing.
- **Dynamic node management:** Add or remove shards at runtime.
- **Data migration:** When a node is removed, its data is migrated to the next responsible node to prevent data loss.
- **Shard simulation:** Each shard is represented by a separate SQL Server database.
- **Simple user data insert/read example:** Insert and read user names mapped to shards using the consistent hashing ring.

## Technologies Used

- C# (.NET 8.0)
- Entity Framework Core 9.0
- SQL Server Express
- MD5 hashing algorithm for consistent hashing
- Visual Studio 2022

## How It Works

- The consistent hash ring manages a sorted dictionary of hash values mapped to nodes.
- Each physical node has multiple virtual nodes identified by a unique string hash.
- When a key (e.g., username) is inserted or retrieved, the hash of the key determines which node it belongs to.
- When a node is removed, the data assigned to it is migrated to the next node in the ring to ensure availability.

## Running the Project

1. Ensure SQL Server Express is installed and running on your machine.
2. The project will automatically create and initialize shard databases (`Shard1`, `Shard2`, `Shard3`, `Shard4`).
3. Run the program, which inserts sample user data into shards and reads it back, demonstrating data distribution and migration when a node is removed.

## Example Output

```vbnet
Initialized: Server=localhost\SQLEXPRESS;Database=Shard1;...
Initialized: Server=localhost\SQLEXPRESS;Database=Shard2;...
Initialized: Server=localhost\SQLEXPRESS;Database=Shard3;...
Initialized: Server=localhost\SQLEXPRESS;Database=Shard4;...
All shards initialized.
Inserted User Alice into Shard2
Inserted User Bob into Shard3
Inserted User Charlie into Shard4
Inserted User Diana into Shard4
Reading users from correct shard:
User (Alice) is in Shard2
User (Bob) is in Shard3
User (Charlie) is in Shard4
User (Diana) is in Shard4
Removed Shard3, migrating data...
Reading users from correct shard:
User (Alice) is in Shard2
User (Bob) is in Shard4
User (Charlie) is in Shard4
User (Diana) is in Shard4
