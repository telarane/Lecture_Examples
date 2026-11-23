namespace LogHub.Graphs;

public static class GraphExtensions
{
    // Возвращает независимый итератор для обхода в глубину
    public static IEnumerable<Node> DepthFirst(this Node root) 
        => new DepthFirstGraph(root);

    // Возвращает независимый итератор для обхода в ширину
    public static IEnumerable<Node> BreadthFirst(this Node root) 
        => new BreadthFirstGraph(root);
}