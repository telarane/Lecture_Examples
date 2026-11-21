using System.Collections;

namespace LogHub.Graphs.Iterators;

public class YieldDfsEnumerable : IEnumerable<Node>
{
    private readonly Node _startNode;
    public YieldDfsEnumerable(Node startNode) => _startNode = startNode;

    public IEnumerator<Node> GetEnumerator()
    {
        var visited = new HashSet<int>();
        var stack = new Stack<Node>();
        stack.Push(_startNode);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (visited.Contains(node.Id)) continue;
            
            visited.Add(node.Id);
            
            // Компилятор автоматически развернет это в State Machine
            yield return node; 

            foreach (var child in node.ChildNodes.Reverse())
            {
                if (!visited.Contains(child.Id))
                    stack.Push(child);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}