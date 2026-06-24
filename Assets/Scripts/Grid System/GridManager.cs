using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]

    [SerializeField] private LevelData currentLevel;
    private float cellSize = 1f;

    private GridCell[,] grid;

    [SerializeField] private GameObject Tile;
    

    public static GridManager instance;
    private void Awake()
    {
        instance = this;
        GenerateGrid();
        SpawnBlocks();
    }

    private void GenerateGrid()
    {
        grid = new GridCell[currentLevel.columns, currentLevel.rows];

        for (int x = 0; x < currentLevel.columns; x++)
        {
            for (int y = 0; y < currentLevel.rows; y++)
            {
                Instantiate(Tile, new Vector3( x, 0, y) , Quaternion.identity , transform);
                grid[x, y] = new GridCell(x, y);
            }
        }

        Debug.Log($"Grid created: {currentLevel.columns}x{currentLevel.rows}");
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3( x, 0,  y) * cellSize + transform.position;
    }

    public Vector2Int GetGridposition(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - transform.position.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.z - transform.position.z) / cellSize);
        return new Vector2Int(x, y);
    }

    public GridCell GetCell(int x, int y)
    {
        if (x >= 0 && x < currentLevel.columns && y >= 0 && y < currentLevel.rows)
            return grid[x, y];
        return null; // out of bounds
    }

    public bool CanMoveToCell(int x, int y)
    {
        GridCell cell = GetCell(x, y);
        if (cell == null) return false;

        return !cell.isOccupied;
    }

    private void SpawnBlocks()
    {
        foreach (var block in currentLevel.blocks)
        {
            Vector3 spawnPos = GetWorldPosition(
                block.position.x,
                block.position.y);

            GameObject blockObj = Instantiate(
                block.blockData.BlockPrefab,
                spawnPos,
                Quaternion.identity);

            Block blockScript = blockObj.GetComponent<Block>();

            blockScript.GridPosition = block.position;

            GetCell(
                block.position.x,
                block.position.y)
                .isOccupied = true;
        }
    }

}
