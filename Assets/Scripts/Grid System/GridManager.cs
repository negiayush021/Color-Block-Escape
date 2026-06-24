using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int columns = 10;
    [SerializeField] private int rows = 10;
    private float cellSize = 1f;

    private GridCell[,] grid;

    [SerializeField] private GameObject Tile;

    public static GridManager instance;
    private void Awake()
    {
        instance = this;
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new GridCell[columns, rows];

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Instantiate(Tile, new Vector3(x, 0, y) , Quaternion.identity);
                grid[x, y] = new GridCell(x, y);
            }
        }

        Debug.Log($"Grid created: {columns}x{rows}");
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + transform.position;
    }

    public Vector2Int GetGridposition(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - transform.position.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.z - transform.position.z) / cellSize);
        return new Vector2Int(x, y);
    }

    public GridCell GetCell(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
            return grid[x, y];
        return null; // out of bounds
    }

    public bool CanMoveToCell(int x, int y)
    {
        GridCell cell = GetCell(x, y);
        if (cell == null) return false;

        return !cell.isOccupied;
    }
}
