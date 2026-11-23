namespace LogHub.Graphs;

public static class CycleDetector
{
    public static List<int>? FindCycle(Node startNode)
    {
        var visited = new HashSet<int>();
        var path = new List<int>();
        var inStack = new HashSet<int>();

        if (Dfs(startNode, visited, path, inStack))
        {
            return path;
        }
        return null;
    }

    private static bool Dfs(Node node, HashSet<int> visited, List<int> path, HashSet<int> inStack)
    {
        if (inStack.Contains(node.Id))
        {
            // Нашли цикл! Обрезаем путь до начала цикла
            int index = path.IndexOf(node.Id);
            path.RemoveRange(0, index);
            path.Add(node.Id); // Замыкаем
            return true;
        }

        if (visited.Contains(node.Id)) return false;

        visited.Add(node.Id);
        path.Add(node.Id);
        inStack.Add(node.Id);

        foreach (var child in node.ChildNodes)
        {
            if (Dfs(child, visited, path, inStack)) return true;
        }

        path.RemoveAt(path.Count - 1);
        inStack.Remove(node.Id);
        return false;
    }
}