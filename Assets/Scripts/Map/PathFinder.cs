using System.Collections.Generic;
using System.Linq;
using Game.Battle.Map;
using UnityEngine;

namespace Game
{
    /**
     * Provides path finding algorithm
     */
    public class PathFinder
    {
        private Grid<AStarNode> _grid;

        public PathFinder(BattleBoard board)
        {
            _grid = new Grid<AStarNode>(board.size, board.size, 1f, Vector3.zero,
                (grid, x, y) => new AStarNode(grid, x, y)
            );
        }

        public List<AStarNode> AStar(int startX, int startY, int endX, int endY)
        {
            AStarNode startNode = _grid.GetValue(startX, startY);
            AStarNode endNode = _grid.GetValue(endX, endY);
            List<AStarNode> open = new List<AStarNode> { startNode };
            List<AStarNode> closed = new List<AStarNode>();

            startNode.gcost = 0;
            startNode.hcost = startNode.CostTo(endX, endY);

            while (open.Count > 0)
            {
                AStarNode currentNode = open.OrderBy(node => node.fcost).First();
                if (currentNode == endNode)
                    return ConstructPath(endNode);

                open.Remove(currentNode);
                closed.Add(currentNode);

                foreach (AStarNode neighbour in currentNode.GetNeighbours())
                {
                    if (closed.Contains(neighbour)) continue;

                    int tentativeGCost = currentNode.gcost + currentNode.CostTo(neighbour);
                    if (tentativeGCost < neighbour.gcost)
                    {
                        neighbour.parent = currentNode;
                        neighbour.gcost = tentativeGCost;
                        neighbour.hcost = neighbour.CostTo(endNode);

                        if (!open.Contains(neighbour))
                            open.Add(neighbour);
                    }
                }
            }

            return null;
        }

        private List<AStarNode> ConstructPath(AStarNode endNode)
        {
            List<AStarNode> path = new List<AStarNode>{ endNode };
            AStarNode currentNode = endNode;
            while (currentNode.parent != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            return path;
        }
        
        /**
         * A inner class for pathfinding
         */
        public class AStarNode
        {
            private Grid<AStarNode> _grid;
            private List<AStarNode> _neighbours;

            public int x { get; private set; }
            public int y { get; private set; }
            public int fcost => hcost + gcost;
            public int hcost;
            public int gcost;
            public AStarNode parent;

            public AStarNode(Grid<AStarNode> grid, int _x, int _y)
            {
                _grid = grid;
                x = _x;
                y = _y;

                gcost = int.MaxValue;
            }

            public int CostTo(AStarNode toNode) => CostTo(toNode.x, toNode.y);
            public int CostTo(int endX, int endY) => Mathf.Abs(x - endX) + Mathf.Abs(y - endY);

            public List<AStarNode> GetNeighbours()
            {
                if (_neighbours != null) return _neighbours;
            
                _neighbours = new List<AStarNode>();
                if (x - 1 >= 0)
                    _neighbours.Add(_grid.GetValue(x - 1, y));
                if (x + 1 < _grid.width)
                    _neighbours.Add(_grid.GetValue(x + 1, y));
                if (y - 1 >= 0)
                    _neighbours.Add(_grid.GetValue(x, y - 1));
                if (y + 1 < _grid.height)
                    _neighbours.Add(_grid.GetValue(x, y + 1));
                
                return _neighbours;
            }
        }
    }
}