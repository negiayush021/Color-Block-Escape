using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelData currentLevel;
    public GameObject levelCompleteScreen;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private GameObject[] particles;

    [SerializeField] private GameObject RawImage;
    [SerializeField] private GameObject GiftGlow;
    [SerializeField] private GameObject Nextlvlbtn;


    float Toatl_Time_Of_Active_Level;
    float Remaining_Time_Of_Active_Level;

    public event Action OnLevelComplete;
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


        if (MovesRemaining <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void RestoreMove()
    {
        MovesRemaining++;
    }

    public void ShowLevelComplete()
    {
        OnLevelComplete?.Invoke();

        levelCompleteScreen.SetActive(true);
        StartCoroutine(showing());
    }

    IEnumerator showing()
    {
        

        yield return new WaitForSeconds(0.5f);

        int count = 0;
        float percentage = Remaining_Time_Of_Active_Level / Toatl_Time_Of_Active_Level * 100;
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

}