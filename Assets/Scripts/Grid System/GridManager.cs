using NUnit.Framework;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]

    [SerializeField] private LevelData currentLevel;
    [SerializeField] private Gate[] gatePrefabs;

    private float cellSize = 1f;

    private GridCell[,] grid;

    [SerializeField] private GameObject Tile;
    

    public static GridManager instance;
    private void Awake()
    {
        instance = this;
        //GameManager.instance.OnLevelChangeBtnPressed += LevelChange;

    }

    private void Start()
    {
        currentLevel =
        GameManager.instance.CurrentLevelData;

        GenerateGrid();
        SpawnGates();
        SpawnBlocks();

        GameManager.instance.OnLevelChangeBtnPressed += LevelChange;
        GameManager.instance.OnRestartBtnPressed += Restart;
    }

    /*private void OnEnable()
    {
        GameManager.instance.OnLevelChangeBtnPressed += LevelChange;
    }*/

    private void OnDisable()
    {
        GameManager.instance.OnLevelChangeBtnPressed -= LevelChange;
        GameManager.instance.OnRestartBtnPressed -= Restart;
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

    public void DestroyGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        grid = null;
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

    private void SpawnGates()
    {
        foreach (var gateData in currentLevel.gates)
        {
            Gate prefab = GetGatePrefab(gateData.gateColor);

            Gate gate = Instantiate(
                prefab,
                GetWorldPosition(
                    gateData.position.x,
                    gateData.position.y),
                Quaternion.identity);

            gate.gateColor = gateData.gateColor;

            GridCell cell = GetCell(
                gateData.position.x,
                gateData.position.y);

            cell.cellType = CellType.Exit;
            cell.ExitColor = gateData.gateColor;
            cell.gateReference = gate;

            if (gateData.position.x == 0)
            {
                gate.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                if (gateData.position.x == currentLevel.rows - 1)
                    continue;

                if (gateData.position.y == currentLevel.columns-1)
                {
                    gate.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                else
                {
                    gate.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
            }
            
        }
    }

    private Gate GetGatePrefab(BlockColor color)
    {
        foreach (var gate in gatePrefabs)
        {
            if (gate.gateColor == color)
                return gate;
        }

        Debug.LogError($"No gate prefab found for {color}");
        return null;
    }

    private void LevelChange()
    {
        DestroyBlocks();
        DestroyGrid();
        currentLevel = GameManager.instance.CurrentLevelData;

        GenerateGrid();
        SpawnGates();
        SpawnBlocks();
    }

    public void DestroyBlocks()
    {
        Block[] blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);

        foreach(Block block in blocks)
        {
            Destroy(block.gameObject);
        }
    }

    private void Restart()
    {
        DestroyBlocks();
        SpawnBlocks();
        ClearGridOccupancy();

    }
    public void ClearGridOccupancy()
    {
        for (int x = 0; x < currentLevel.columns; x++)
        {
            for (int y = 0; y < currentLevel.rows; y++)
            {
                grid[x, y].isOccupied = false;
            }
        }
    }
}
