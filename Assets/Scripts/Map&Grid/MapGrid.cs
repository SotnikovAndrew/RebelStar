
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    public Dictionary<Vector2, CellProp> datacell;
    public Dictionary<Vector2, Node> _GridNode;
    private GameObject[] _trees;
    private GameObject[] _stones;
    private GameObject[] _bushes;

    private void Awake()
    {
        datacell = new Dictionary<Vector2, CellProp>();
        _GridNode = new Dictionary<Vector2, Node>();
        
        var allCell = FindObjectsOfType<CellProp>();
        foreach (var props in allCell)
        {
            var position = props.transform.position;
            datacell.Add(position,props);
            _GridNode.Add(position, new Node(position,this));
        }
        
        foreach (var node in _GridNode)
        {
            node.Value.ActiveNeighburs();
        }
        
        var players =FindObjectsOfType<Player>();
        foreach (var player in players)
        {
            datacell[player.transform.position].walkable = false;
        }

        _trees = GameObject.FindGameObjectsWithTag("Trees");
        foreach (var trees in _trees)
        {
            datacell[trees.transform.position].walkable = false;
        }
        
        _bushes = GameObject.FindGameObjectsWithTag("Bushes");
        foreach (var bush in _bushes)
        {
            datacell[bush.transform.position].walkable = false;
        }
        
        _stones = GameObject.FindGameObjectsWithTag("Stones");
        foreach (var stone in _stones)
        {
            datacell[stone.transform.position].walkable = false;
        }
        print("vse gotovo");
        

    }
}
