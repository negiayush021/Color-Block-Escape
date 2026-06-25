using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public int columns;
    public int rows;
    public int moveLimit;
    public int No_of_Blocks;

    public Vector3 cameraPos;

    public BlockSpawnData[] blocks;
    public GateSpawnData[] gates;
}
