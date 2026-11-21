using System.Collections;

namespace LogHub.Graphs.Iterators;

public class ManualDfsIterator : IEnumerator<Node>
{
    private readonly Node _startNode;
    private readonly Stack<Node> _stack = new();
    private readonly HashSet<int> _visited = new();
    private Node? _current;

    public ManualDfsIterator(Node startNode)
    {
        _startNode = startNode;
        Reset();
    }

    public Node Current => _current!;
    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        while (_stack.Count > 0)
        {
            var node = _stack.Pop();
            if (_visited.Contains(node.Id)) continue;

            _current = node;
            _visited.Add(node.Id);

            foreach (var child in node.ChildNodes.Reverse())
            {
                if (!_visited.Contains(child.Id))
                    _stack.Push(child);
            }
            return true;
        }
        return false;
    }

    public void Reset()
    {
        _stack.Clear();
        _visited.Clear();
        _stack.Push(_startNode);
        _current = null;
    }

    public void Dispose()
    {
        _stack.Clear();
        _visited.Clear();
    }
}

// Обертка для использования в foreach
public class ManualDfsEnumerable : IEnumerable<Node>
{
    private readonly Node _startNode;
    public ManualDfsEnumerable(Node startNode) => _startNode = startNode;
    
    public IEnumerator<Node> GetEnumerator() => new ManualDfsIterator(_startNode);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}