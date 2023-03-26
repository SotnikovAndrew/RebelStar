
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PathFinder 
{
    private MapGrid _Grid;
    private Dictionary<Vector2,Node> CheckedNodes;
    private Dictionary<Vector2,Node> WaitingNodes;

    public PathFinder(MapGrid grid)
    {
        CheckedNodes = new Dictionary<Vector2,Node>();
        WaitingNodes = new Dictionary<Vector2,Node>();
        _Grid = grid;
    }

    public List<Vector2> GetPath(Vector2 start, Vector2 targetPosition)
    {
        ResetNodes();
        var pathToTarget = new List<Vector2>();
        if (start == targetPosition)
            return pathToTarget;
        Node startNode = _Grid._GridNode[start];
        foreach (var neib in startNode.Neighbours)
        {
            if (neib != null)
            {
                neib.SetActive(targetPosition,startNode,startNode.G);
                WaitingNodes.Add(neib.Position,neib);
            }
           
        }
        CheckedNodes.Add(startNode.Position,startNode);
        while (WaitingNodes.Count > 0)
        {
            Node nodetocheck = FindCheckNode(WaitingNodes);
            if (nodetocheck.Position == targetPosition)
            {
               return pathToTarget = CalculatePath(nodetocheck);
                
            }

            if (_Grid.datacell[nodetocheck.Position].walkable)
            {
                if (!CheckedNodes.ContainsKey(nodetocheck.Position))
                {
                    CheckedNodes.Add(nodetocheck.Position,nodetocheck);
                    foreach (var neib in nodetocheck.Neighbours)
                    {
                        if (neib != null && !WaitingNodes.ContainsKey(neib.Position)&&!CheckedNodes.ContainsKey(neib.Position)) //  vot tut iskluchau dublicati NOD
                        {
                            neib.SetActive(targetPosition,nodetocheck,nodetocheck.G);
                            WaitingNodes.Add(neib.Position,neib);
                        }
                    }
                } 
                else
                {
                    WaitingNodes.Remove(nodetocheck.Position);

                }
            }
            else
            {
                WaitingNodes.Remove(nodetocheck.Position);
            }
            
        }
        
        return null;
    }
    

    private Node FindCheckNode(Dictionary<Vector2, Node> dictionary)
    {
        float H = 10000;
        Node toCheck = null;
        foreach (var node in dictionary)
        {
            if (node.Value.H < H)
            {
                toCheck = node.Value;
                H = node.Value.H;
            }
        }

        return toCheck;
    }

    private void ResetNodes()
    {
        foreach (var node in CheckedNodes)
        {
            node.Value.ResetNode();
        }
        foreach (var node in WaitingNodes)
        {
            node.Value.ResetNode();
        }
        WaitingNodes.Clear();
        CheckedNodes.Clear();
    }

    private List<Vector2> CalculatePath(Node node)
    {
        var path = new List<Vector2>();
        Node currentNode = node;
        while (currentNode.PreviosNode !=null)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.PreviosNode;
        }
        

        return path;
    }

}
