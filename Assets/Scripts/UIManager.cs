using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TMP_Text movesText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateMovesUI(int moves)
    {
        movesText.text = moves.ToString();
    }
}