using UnityEngine;

public enum BlockType
{
    Normal,
    Gate,
    Obstacle
}

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    public string BlockName;
    public BlockType blockType;
    public GameObject BlockPrefab;
    public BlockColor blockColor;
}
