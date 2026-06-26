using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        instance = this;
    }

    public Transform cameraTransform;

    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject homePage;


    [SerializeField] private Image LoadingFadeIMG;

    public event Action OnLevelChangeBtnPressed;
    public event Action OnRestartBtnPressed;

    public LevelData[] leveldata;
    public int currentLevel;

    public GameObject[] Designs;
    private void Start()
    {
        StartCoroutine(startingTheGame());
        foreach(GameObject design in Designs)
        {
            design.SetActive(false);
        }
        Designs[currentLevel].SetActive(true);
        cameraTransform.position = CurrentLevelData.cameraPos;
        
    }

    IEnumerator startingTheGame()
    {
        for(float t = 1; t> 0; t -= Time.deltaTime)
        {
            Color c = LoadingFadeIMG.color;
            c.a = t;
            LoadingFadeIMG.color = c;
            yield return null;
        }
    }

    public LevelData CurrentLevelData
    {
        get { return leveldata[currentLevel]; }
    }

    public void OpenSettingsPage()
    {
        settingsPage.SetActive(true);
    }

    public void OpenHomePage()
    {
        homePage.SetActive(true);
    }

    public void ClosePages()
    {
        homePage.SetActive(false);
        settingsPage.SetActive(false);
    }

    public void NextLevel()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        currentLevel++;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            Color c = LoadingFadeIMG.color;
            c.a = t;
            LoadingFadeIMG.color = c;
            yield return null;
        }

        StartCoroutine(startingTheGame());
        OnLevelChangeBtnPressed?.Invoke();

        foreach (GameObject design in Designs)
        {
            design.SetActive(false);
        }
        Designs[currentLevel].SetActive(true);
        cameraTransform.position = CurrentLevelData.cameraPos;

    }

    public void RestartTheGame()
    {
        StartCoroutine(Restarting());
        
    }

    IEnumerator Restarting()
    {
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            Color c = LoadingFadeIMG.color;
            c.a = t;
            LoadingFadeIMG.color = c;
            yield return null;
        }
        OnRestartBtnPressed?.Invoke();

        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            Color c = LoadingFadeIMG.color;
            c.a = t;
            LoadingFadeIMG.color = c;
            yield return null;
        }
    }

    public void ReturnToMenu()
    {
        StartCoroutine(Returning());
    }
    IEnumerator Returning()
    {
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            Color c = LoadingFadeIMG.color;
            c.a = t;
            LoadingFadeIMG.color = c;
            yield return null;
        }
        SceneManager.LoadScene("Menu");
    }

}
