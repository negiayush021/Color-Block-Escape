using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;


    [SerializeField] private LevelData currentLevel;

    public GameObject levelCompleteScreen;
    public GameObject YouLoseScreen;

    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject[] particles;

    [SerializeField] private GameObject RawImage;
    [SerializeField] private GameObject GiftGlow;
    [SerializeField] private GameObject Nextlvlbtn;

    [SerializeField] private int Level_number;
    [SerializeField] private TextMeshProUGUI Level_number_Text;


    float Toatl_Moves_Of_Active_Level;
    float Remaining_Moves_Of_Active_Level;

    public event Action OnLevelComplete;
    public int MovesRemaining { get; private set; }
    public int BlocksRemaining { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentLevel =
        GameManager.instance.CurrentLevelData;

        MovesRemaining = currentLevel.moveLimit;
        BlocksRemaining = currentLevel.No_of_Blocks;
        Level_number = GameManager.instance.currentLevel + 1;

        Level_number_Text.text = "Level " + Level_number.ToString();

        UIManager.Instance.UpdateMovesUI(MovesRemaining);
    }

    private void OnEnable()
    {
        GameManager.instance.OnLevelChangeBtnPressed += LevelChange;
        GameManager.instance.OnRestartBtnPressed += Restart;
    }

    private void OnDisable()
    {
        GameManager.instance.OnLevelChangeBtnPressed -= LevelChange;
        GameManager.instance.OnRestartBtnPressed -= Restart;
    }

    public void UseMove()
    {
        MovesRemaining--;
        UIManager.Instance.UpdateMovesUI(MovesRemaining);

        if (MovesRemaining <= 0)
        {
            YouLoseScreen.SetActive(true);
            Debug.Log("Game Over");
        }
    }

    public void RestoreMove()
    {
        MovesRemaining++;
        UIManager.Instance.UpdateMovesUI(MovesRemaining);
    }

    public void ShowLevelComplete()
    {
        OnLevelComplete?.Invoke();

        levelCompleteScreen.SetActive(true);
        StartCoroutine(showing());
    }

    IEnumerator showing()
    {
        Remaining_Moves_Of_Active_Level = MovesRemaining;
        Toatl_Moves_Of_Active_Level = currentLevel.moveLimit;

        yield return new WaitForSeconds(0.5f);

        int count = 0;
        float percentage = Remaining_Moves_Of_Active_Level / Toatl_Moves_Of_Active_Level * 100;
        if (percentage > 75)
        {
            foreach (var star in stars)
            {
                if (count == 3)
                    break;

                star.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                particles[count].SetActive(true);
                count++;
            }
        }
        else if (percentage > 50)
        {
            foreach (var star in stars)
            {
                if (count == 2)
                    break;

                star.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                particles[count].SetActive(true);
                count++;
            }
        }
        else
        {
            foreach (var star in stars)
            {
                if (count == 1)
                    break;

                star.SetActive(true);
                yield return new WaitForSeconds(0.4f);
                particles[count].SetActive(true);
                count++;
            }
        }

        GiftGlow.SetActive(true);
        RawImage img = RawImage.GetComponent<RawImage>();
        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            Color c = img.color;
            c.a = t;
            img.color = c;
            yield return null;
        }

        Nextlvlbtn.SetActive(true);

    }

    public void HideLevelComplete()
    {
        levelCompleteScreen.SetActive(false);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(false);
            particles[i].SetActive(false);
        }

        GiftGlow.SetActive(false);
        Nextlvlbtn.SetActive(false);
    }

    private void LevelChange()
    {

        currentLevel = GameManager.instance.CurrentLevelData;
        HideLevelComplete();
        MovesRemaining = currentLevel.moveLimit;
        BlocksRemaining = currentLevel.No_of_Blocks;
        UIManager.Instance.UpdateMovesUI(MovesRemaining);
        Level_number = GameManager.instance.currentLevel + 1;
        Level_number_Text.text = "Level " + Level_number.ToString();
    }

    public void CheckLevelComplete()
    {
        if (BlocksRemaining == 0)
        {
            ShowLevelComplete();
        }
    }

    public void DecreaseBlocksNumber()
    {
        BlocksRemaining--;
    }

    private void Restart()
    {
        MovesRemaining = currentLevel.moveLimit;
        UIManager.Instance.UpdateMovesUI(MovesRemaining);
        YouLoseScreen.SetActive(false);
    }
   
}