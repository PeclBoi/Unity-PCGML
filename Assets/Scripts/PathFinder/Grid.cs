using System;
using UnityEngine;

public class Grid<TGridElement>
{

    private int width;
    private int height;
    private float cellSize;
    private TGridElement[,] gridArray;
    private Vector3 originPosition;

    public int Witdh { get { return width; } }
    public int Height { get { return height; } }
    public Vector3 OriginPosition { get { return originPosition; } }

    public Grid(int width, int height, float cellSize, Vector3 origin, Func<Grid<TGridElement>, int, int, TGridElement> createGridElement)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new TGridElement[width, height];
        this.originPosition = origin;

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                bool isWalkable = !IsSpaceFree(GetWorldPosition(x, y));
                gridArray[x, y] = createGridElement(this, x, y);

                Debug.Log(x + ", " + y);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 1000);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 1000);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 1000);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 1000);
    }

    private bool IsSpaceFree(Vector3 worldPos)
    {
        Vector3 center = ConvertWorldPosToCellCenter(worldPos);
        return !Physics.Raycast(center, Vector3.up);
    }

    private Vector3 ConvertWorldPosToCellCenter(Vector3 worldPos)
    {
        return new Vector3(worldPos.x + cellSize / 2, 0, worldPos.z + cellSize / 2);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize + originPosition.x, 0, y * cellSize + originPosition.z);
    }

    public TGridElement GetValue(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return gridArray[x, y];
        }

        return default;
    }

    public void SetValue(int x, int y, TGridElement value)
    {
        gridArray[x, y] = value;
    }


}
