using UnityEngine;



    public class Node
    {
        public Vector2 Position;
        public Vector2 TargetPosition;
        public Node PreviosNode = null;
        public Node[] Neighbours;
        public float F;
        public float G;
        public float H;
        private MapGrid _grid;
  
        public Node( Vector2 nodePosition, MapGrid grid)
        {
            Position = nodePosition;
            G = 0;
            _grid = grid;
        }

        public void ActiveNeighburs()
        {
            Neighbours = GetNeighboursNodes(Position);
        }

        public void SetActive(Vector2 targetPosition, Node previos, float g)
        {
            G = g + 1;
            PreviosNode = previos; 
     
            H = Vector2.Distance(Position, targetPosition);
        }

        private Node[] GetNeighboursNodes(Vector2 nodePos)
        {
            Node[] _neighburs = new Node[4];
            Vector2[] sosedi = {
                new Vector2(nodePos.x + 1, nodePos.y) ,
                new Vector2(nodePos.x - 1, nodePos.y) ,
                new Vector2(nodePos.x, nodePos.y + 1) ,
                new Vector2(nodePos.x, nodePos.y - 1) ,
                /*new Vector2(nodePos.x + 1, nodePos.y+1) ,
                new Vector2(nodePos.x + 1, nodePos.y-1) ,    // диагональные соседи, потом протестим
                new Vector2(nodePos.x - 1, nodePos.y+1) ,
                new Vector2(nodePos.x -1, nodePos.y-1) ,*/
            };
            for (int i = 0; i < 4 ; i++)
            {
                if (_grid._GridNode.ContainsKey(sosedi[i]))
                {
                    _neighburs[i] = _grid._GridNode[sosedi[i]];

                }
                else
                {
                    _neighburs[i] = null;
                }
            }
            return _neighburs;
        }

        public void ResetNode()
        {
            G = 0;
            H = 0;
            PreviosNode = null;
        }
    }
