
using System.Collections.Generic;
using UnityEngine;

public class MoveZone 
{
    private MapGrid _Grid;
    private PathFinder _finder;
    private List<Vector2> _moveList;
    private Player _currentPlayer;


    public MoveZone(MapGrid grid, PathFinder finder)
    {
        _Grid = grid;
        _finder = finder;
        _moveList = new List<Vector2>();
    }
    public void  CalculateMoveGrid(Player player)
    {
        _currentPlayer = player;
        Vector2 playerPosition = player.transform.position;
        for (int x = (int)playerPosition.x - player._movePoint; x <= (int)playerPosition.x + player._movePoint; x++)
        {
            for (int y = (int)playerPosition.y - player._movePoint; y <= (int)playerPosition.y + player._movePoint; y++)
            {
                Vector2 point = new Vector2(x, y);
                
                if (_Grid.datacell.ContainsKey(point)&&_Grid.datacell[point].walkable)
                {
                    var path = _finder.GetPath(playerPosition, point);


                     if (path!=null && path.Count<=player._movePoint)
                    {
                        _moveList.Add(point);
                        _Grid.datacell[point]._hightLight.SetActive(true);
                    }

                }
            }
        }
    }
    private void ShowPlayerMoveZone(Player obj)
    {
        _Grid.datacell[new Vector2(2, 2)]._hightLight.SetActive(true);
    }

    public void UpdateMoveGrid()
    {
        ClearHighLight();
        CalculateMoveGrid(_currentPlayer);
            
    }
    public void ClearHighLight()
    {
        foreach (var position in _moveList)
        {
            _Grid.datacell[position]._hightLight.SetActive(false);
        }
        _moveList.Clear();
    }

    public bool ConteinPath(Vector2 point)
    {
        return _moveList.Contains(point);
    }
    
}
