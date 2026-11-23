using System.Collections;

namespace LogHub.Graphs;

public class DepthFirstGraph : IEnumerable<Node>
{
    private readonly Node _startNode;

    public DepthFirstGraph(Node startNode) => _startNode = startNode;

    // При каждом вызове GetEnumerator создается НОВЫЙ итератор (новый стек и visited)
    public IEnumerator<Node> GetEnumerator()
    {
        var visited = new HashSet<int>();
        var stack = new Stack<Node>();
        stack.Push(_startNode);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (visited.Contains(current.Id)) continue;

            visited.Add(current.Id);
            yield return current;

            // Reverse нужен, чтобы обход шел слева направо (как в учебнике)
            foreach (var child in current.ChildNodes.Reverse())
            {
                if (!visited.Contains(child.Id))
                    stack.Push(child);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}