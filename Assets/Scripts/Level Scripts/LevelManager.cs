using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelData currentLevel;

    public int MovesRemaining { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MovesRemaining = currentLevel.moveLimit;
    }

    public void UseMove()
    {
        MovesRemaining--;

        Debug.Log("Moves Remaining : " + MovesRemaining);

        if (MovesRemaining <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void RestoreMove()
    {
        MovesRemaining++;
        Debug.Log("Moves Remaining : " + MovesRemaining);
    }
}