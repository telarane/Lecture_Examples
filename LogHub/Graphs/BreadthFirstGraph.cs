using System.Collections;

namespace LogHub.Graphs;

public class BreadthFirstGraph : IEnumerable<Node>
{
    private readonly Node _startNode;

    public BreadthFirstGraph(Node startNode) => _startNode = startNode;

    public IEnumerator<Node> GetEnumerator()
    {
        var visited = new HashSet<int>();
        var queue = new Queue<Node>();
        queue.Enqueue(_startNode);
        visited.Add(_startNode.Id);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;

            foreach (var child in current.ChildNodes)
            {
                if (!visited.Contains(child.Id))
                {
                    visited.Add(child.Id);
                    queue.Enqueue(child);
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}