using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Game
{
    public class Grid<TGridObject>
    {
        public int width { get; private set; }
        public int height { get; private set; }
        public float cellSize { get; private set; }
        public Vector3 originPosition { get; private set; }

        private TGridObject[,] gridArray;

        public TGridObject this[int x, int y]
        {
            get => GetValue(x, y); 
        }
        
        public TGridObject this[Vector2 v]
        {
            get => GetValue((int) v.x, (int) v.y); 
        }

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createTGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new TGridObject[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    gridArray[i, j] = createTGridObject(this, i, j);
                }
            }
        }

        public void DebugDraw(float dur = 100f)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, dur);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, dur);
                }
            }
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, dur);
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, dur);

        }

        public Vector3 GetWorldPosition(Vector3 pos)
        {
            return GetWorldPosition((int)pos.x, (int)pos.y);
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return originPosition + new Vector3(x * cellSize, y * cellSize, 0);
        }

        public Vector3Int GetGridPosition(int x, int y)
        {
            return GetGridPosition(new Vector3(x, y, 0));
        }

        public Vector3Int GetGridPosition(Vector3 pos)
        {
            return Vector3Int.FloorToInt(pos - originPosition);
        }

        public TGridObject GetValue(int x, int y) => gridArray[x, y];

        public TGridObject GetValue(Vector3 pos) => GetValue((int)pos.x, (int)pos.y);

        public void SetValue(int x, int y, TGridObject gridObject)
        {
            gridArray[x, y] = gridObject;
        }

        public int TileCount => width * height;

        public HashSet<TGridObject> GetNeigbhoursOfTile(int gx, int gy) {
            HashSet<TGridObject> neighbours = new HashSet<TGridObject>();
            if (gx - 1 >= 0)
                neighbours.Add(GetValue(gx - 1, gy));
            if (gx + 1 < width)
                neighbours.Add(GetValue(gx + 1, gy));
            if (gy - 1 >= 0)
                neighbours.Add(GetValue(gx, gy - 1));
            if (gy + 1 < height)
                neighbours.Add(GetValue(gx, gy + 1));

            return neighbours;
        }

        public void Map(Action<TGridObject> func)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    func.Invoke(gridArray[i, j]);
                }
            }
        }
    }
}
