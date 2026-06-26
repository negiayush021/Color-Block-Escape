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

        currentLevel = PlayerPrefs.GetInt("Open Level") - 1;
    }

    public Transform cameraTransform;

    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject homePage;


    [SerializeField] private Image LoadingFadeIMG;

    public bool PowerInUse = false;

    public event Action OnLevelChangeBtnPressed;
    public event Action OnRestartBtnPressed;

    public LevelData[] leveldata;
    public int currentLevel;

    public GameObject[] Designs;

    public GameObject Hammer_prefab;
    public GameObject smoke_effect;
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

    public void destroyObstacle(GameObject block)
    {
        if(block.tag == "Obstacle")
        {
            StartCoroutine(destroyingObstacle(block));
        }
    }

    IEnumerator destroyingObstacle(GameObject block)
    {
        GameObject hammer = Instantiate(Hammer_prefab, new Vector3(block.transform.position.x, block.transform.position.y + 1, block.transform.position.z - .5f), Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        //MusicManager.instance.PlayClip(17);
        yield return new WaitForSeconds(.2f);
        Destroy(hammer);
        GameObject smoke = Instantiate(smoke_effect, new Vector3(block.transform.position.x, block.transform.position.y + 1, block.transform.position.z), Quaternion.identity);
        Destroy(block.gameObject);
    }

}
