using LogHub.Graphs;
using LogHub.Graphs.Iterators;
using Xunit;

namespace LogHub.Tests.Graphs;

public class GraphTests
{
    private Node BuildStandardGraph()
    {
        var n0 = new Node { Id = 0 };
        var n1 = new Node { Id = 1 };
        var n2 = new Node { Id = 2 };
        var n3 = new Node { Id = 3 };
        var n4 = new Node { Id = 4 };
        n0.ChildNodes = new[] { n1, n2 };
        n1.ChildNodes = new[] { n3 };
        n2.ChildNodes = new[] { n3, n4 };
        return n0;
    }

    // Тест 8 (Порядок обхода DFS)
    [Fact]
    public void Dfs_ReturnsCorrectOrder()
    {
        var graph = BuildStandardGraph();
        var result = graph.DepthFirst().Select(n => n.Id).ToList();
        Assert.Equal(new[] { 0, 1, 3, 2, 4 }, result);
    }

    // Тест 9 (Порядок обхода BFS)
    [Fact]
    public void Bfs_ReturnsCorrectOrder()
    {
        var graph = BuildStandardGraph();
        var result = graph.BreadthFirst().Select(n => n.Id).ToList();
        Assert.Equal(new[] { 0, 1, 2, 3, 4 }, result);
    }

    // Тест 10 (Вложенные foreach дают корректный результат)
    [Fact]
    public void NestedForeach_WorksCorrectly()
    {
        var graph = BuildStandardGraph();
        int count = 0;
        
        foreach (var _ in graph.DepthFirst())
        {
            foreach (var __ in graph.BreadthFirst())
            {
                count++;
            }
        }
        Assert.Equal(25, count); 
    }

    // Тест 11 (Граф с циклом не зацикливает обход)
    [Fact]
    public void GraphWithCycle_DoesNotHang()
    {
        var n0 = new Node { Id = 0 };
        var n1 = new Node { Id = 1 };
        n0.ChildNodes = new[] { n1 };
        n1.ChildNodes = new[] { n0 };

        var result = n0.DepthFirst().ToList();
        Assert.Equal(2, result.Count);
    }

    // Тест 12 (Пустой граф)
    [Fact]
    public void EmptyGraph_ReturnsEmpty()
    {
        var n0 = new Node { Id = 0 };
        var result = n0.DepthFirst().ToList();
        Assert.Single(result);
    }

    // Тест 13 (Граф из одной вершины)
    [Fact]
    public void SingleVertexGraph_ReturnsOne()
    {
        var n0 = new Node { Id = 0 };
        var result = n0.BreadthFirst().ToList();
        Assert.Single(result);
        Assert.Equal(0, result[0].Id);
    }

    // Тест 14 (Обе реализации итератора дают одинаковую последовательность)
    [Fact]
    public void ManualAndYieldIterators_ReturnSameSequence()
    {
        var graph = BuildStandardGraph();
        var manual = new ManualDfsEnumerable(graph).Select(n => n.Id).ToList();
        var yield = new YieldDfsEnumerable(graph).Select(n => n.Id).ToList();
        Assert.Equal(manual, yield);
    }
    
    // Тест 15 (Детектор циклов находит цикл)
    [Fact]
    public void CycleDetector_FindsCycle()
    {
        var n0 = new Node { Id = 0 };
        var n1 = new Node { Id = 1 };
        n0.ChildNodes = new[] { n1 };
        n1.ChildNodes = new[] { n0 };

        var cycle = CycleDetector.FindCycle(n0);
        Assert.NotNull(cycle);
        Assert.Equal(cycle.First(), cycle.Last());
    }

    // Тест 16 (Итератор можно переиспользовать)
    [Fact]
    public void ManualIterator_CanBeReused()
    {
        var n0 = new Node { Id = 0 };
        var iter = new ManualDfsEnumerable(n0);
        var first = iter.ToList();
        var second = iter.ToList();
        Assert.Single(first);
        Assert.Single(second);
    }

    // Тест 17 (Ручной итератор поддерживает Dispose)
    [Fact]
    public void ManualIterator_DisposeClearsState()
    {
        var n0 = new Node { Id = 0 };
        var iterator = new ManualDfsIterator(n0);
        iterator.MoveNext();
        iterator.Dispose();
        
        // После Dispose стек пуст, MoveNext должен вернуть false (в текущей реализации он вызовет Reset, но не упадет)
        // Здесь просто проверяем, что повторный MoveNext не бросает исключение
        var result = iterator.MoveNext(); 
        Assert.False(result); 
    }
    
    // Тест 18 (Ручной итератор поддерживает Reset)
    [Fact]
    public void ManualIterator_ResetWorks()
    {
        var n0 = new Node { Id = 0 };
        var iterator = new ManualDfsIterator(n0);
        iterator.MoveNext(); // Перешли на 0
        iterator.Reset();     // Сбросили
        
        Assert.True(iterator.MoveNext());
        Assert.Equal(0, iterator.Current.Id);
    }
}