using System.Security.Cryptography;
using System.Text;

namespace ConsistentHashing
{
    public class ConsistentHash<TNode>
    {
        private readonly SortedDictionary<int, TNode> _circle = new();
        private readonly int _replicas;
        private readonly Dictionary<TNode, List<int>> _nodeHashes = new();

        public ConsistentHash(int virtualNodes = 100)
        {
            _replicas = virtualNodes;
        }

        public void AddNode(TNode node)
        {
            if (_nodeHashes.ContainsKey(node)) return;

            var hashes = new List<int>();

            for (int i = 0; i < _replicas; i++)
            {
                int hash = ComputeHash($"{node}-VN{i}");
                _circle[hash] = node;
                hashes.Add(hash);
            }

            _nodeHashes[node] = hashes;
        }

        public void RemoveNode(TNode node)
        {
            if (!_nodeHashes.ContainsKey(node)) return;

            foreach (var hash in _nodeHashes[node])
            {
                _circle.Remove(hash);
            }

            _nodeHashes.Remove(node);
        }

        public TNode GetNode(string key)
        {
            if (_circle.Count == 0)
                throw new InvalidOperationException("No nodes added.");

            int hash = ComputeHash(key);
            var tail = _circle.Keys.Where(h => h >= hash).ToList();

            int selectedHash = tail.Count > 0 ? tail[0] : _circle.Keys.First();
            return _circle[selectedHash];
        }

        private int ComputeHash(string input)
        {
            using var md5 = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(bytes);
            return BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
        }
    }
}
