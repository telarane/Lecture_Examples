using System;

namespace LogHub.Graphs;

public class Node
{
    public int Id { get; set; }
    public Node[] ChildNodes { get; set; } = Array.Empty<Node>();

    public override bool Equals(object? obj) => obj is Node other && Id == other.Id;
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => Id.ToString();
}