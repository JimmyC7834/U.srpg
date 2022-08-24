using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game;
using Game.Battle.Map;
using UnityEngine;

public class PathFinder
{
    private Grid<PathFindNode> _grid;

    public PathFinder(BattleBoard board)
    {
        _grid = new Grid<PathFindNode>(board.size, board.size, 1f, Vector3.zero,
            (grid, x, y) => new PathFindNode(grid, x, y)
        );
    }

    public List<PathFindNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathFindNode startNode = _grid.GetValue(startX, startY);
        PathFindNode endNode = _grid.GetValue(endX, endY);
        List<PathFindNode> open = new List<PathFindNode> { startNode };
        List<PathFindNode> closed = new List<PathFindNode>();

        startNode.gcost = 0;
        startNode.hcost = startNode.CostTo(endX, endY);

        while (open.Count > 0)
        {
            PathFindNode currentNode = open.OrderBy(node => node.fcost).First();
            if (currentNode == endNode)
                return ConstructPath(endNode);

            open.Remove(currentNode);
            closed.Add(currentNode);

            foreach (PathFindNode neighbour in currentNode.GetNeighbours())
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

    private List<PathFindNode> ConstructPath(PathFindNode endNode)
    {
        List<PathFindNode> path = new List<PathFindNode>{ endNode };
        PathFindNode currentNode = endNode;
        while (currentNode.parent != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    public class PathFindNode
    {
        private Grid<PathFindNode> _grid;
        private List<PathFindNode> _neighbours;

        public int x { get; private set; }
        public int y { get; private set; }
        public int fcost => hcost + gcost;
        public int hcost;
        public int gcost;
        public PathFindNode parent;

        public PathFindNode(Grid<PathFindNode> grid, int _x, int _y)
        {
            _grid = grid;
            x = _x;
            y = _y;

            gcost = int.MaxValue;
        }

        public int CostTo(PathFindNode toNode) => CostTo(toNode.x, toNode.y);
        public int CostTo(int endX, int endY) => Mathf.Abs(x - endX) + Mathf.Abs(y - endY);

        public List<PathFindNode> GetNeighbours()
        {
            if (_neighbours != null) return _neighbours;
            
            _neighbours = new List<PathFindNode>();
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
