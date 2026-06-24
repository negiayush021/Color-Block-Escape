using UnityEngine;

public class GridCell
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public bool isOccupied;

    public GridCell(int x , int y)
    {
        this.X = x;
        this.Y = y;
        isOccupied = false;
    }
}
