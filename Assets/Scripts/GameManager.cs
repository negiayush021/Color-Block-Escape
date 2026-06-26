using JetBrains.Annotations;
using System;
using System.Collections;
using TMPro;
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

    public event Action OnLevelChangeBtnPressed;
    public event Action OnRestartBtnPressed;


    [Header("References")]
    public Transform cameraTransform;
    [SerializeField] private Image LoadingFadeIMG;
    public GameObject[] Designs;
    public LevelData[] leveldata;
    public int currentLevel;

    [Header("Pages")]

    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject homePage;

    [Space(10)]

    [Header("Hammer Power References")]
    public Button HammerBtn;
    public Image hammer_disable_img;
    public int hammerPowerCount;
    public bool PowerInUse = false;
    public TextMeshProUGUI hammerCount_txt;
    public GameObject ShowingPowerImage;

    [Space(5)]

    public GameObject Hammer_prefab;
    public GameObject smoke_effect;

    [Space(10)]

    [Header("Undo System References")]
    public Button UndoBtn;
    public Image undo_disable_img;
    public int undoCount;
    public TextMeshProUGUI undoCount_txt;


    private void Start()
    {
        
        StartCoroutine(startingTheGame());

        //hammer Power Section

        hammerPowerCount = 1;
        hammerCount_txt.text = hammerPowerCount.ToString();
        hammer_disable_img.fillAmount = 0;
        HammerBtn.enabled = true;
        ShowingPowerImage.SetActive(false);

        // Undo Section

        undoCount = 3;
        undoCount_txt.text = undoCount.ToString();
        undo_disable_img.fillAmount = 0;
        UndoBtn.enabled = true;


        foreach (GameObject design in Designs)
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
        AudioManager.instance.PlayClip(0);
        settingsPage.SetActive(true);
    }

    public void OpenHomePage()
    {
        AudioManager.instance.PlayClip(0);
        homePage.SetActive(true);
    }

    public void ClosePages()
    {
        AudioManager.instance.PlayClip(2);
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

        //hammer Power Section

        hammerPowerCount = 1;
        hammerCount_txt.text = hammerPowerCount.ToString();
        hammer_disable_img.fillAmount = 0;
        HammerBtn.enabled = true;
        ShowingPowerImage.SetActive(false);

        // Undo Section

        undoCount = 3;
        undoCount_txt.text = undoCount.ToString();
        undo_disable_img.fillAmount = 0;
        UndoBtn.enabled = true;

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

        //hammer Power Section

        hammerPowerCount = 1;
        hammerCount_txt.text = hammerPowerCount.ToString();
        hammer_disable_img.fillAmount = 0;
        HammerBtn.enabled = true;
        ShowingPowerImage.SetActive(false);

        // Undo Section

        undoCount = 3;
        undoCount_txt.text = undoCount.ToString();
        undo_disable_img.fillAmount = 0;
        UndoBtn.enabled = true;

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
        AudioManager.instance.PlayClip(1);
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

    public void destroyObstacle(Block block)
    {
        if(block.tag == "Obstacle")
        {
            StartCoroutine(destroyingObstacle(block));

            Vector2Int gridPos = GridManager.instance.GetGridposition(block.transform.position);
            GridCell cell = GridManager.instance.GetCell(gridPos.x, gridPos.y);

            cell.isOccupied = false;

        }
    }

    IEnumerator destroyingObstacle(Block block)
    {
        
        hammerPowerCount--;
        hammerCount_txt.text = hammerPowerCount.ToString();
        PowerInUse = false;
        if(hammerPowerCount == 0)
        {
            hammer_disable_img.fillAmount = 1f;
            HammerBtn.enabled = false;
        }


        GameObject hammer = Instantiate(
            Hammer_prefab, 
            block.transform.position + new Vector3(0,1,-0.5f),
            Quaternion.identity);

        yield return new WaitForSeconds(.5f);
        AudioManager.instance.PlayClip(4);
        yield return new WaitForSeconds(.2f);
        Destroy(hammer);
        GameObject smoke = Instantiate(smoke_effect, new Vector3(block.transform.position.x, block.transform.position.y + 1, block.transform.position.z), Quaternion.identity);
        //Destroy(block.gameObject);
        block.gameObject.SetActive(false);
        ShowingPowerImage.SetActive(false);
    }

    public void UseHammerPower()
    {
        ShowingPowerImage.SetActive(true);
        PowerInUse = true;
    }

    public void UseUndoPower()
    {
        undoCount--;
        undoCount_txt.text = undoCount.ToString();
        if (GameManager.instance.undoCount == 0)
        {
            undo_disable_img.fillAmount = 1f;
            UndoBtn.enabled = false;
        }
    }


    [SerializeField] private Slider music_slider;
    public void MusicBtn()
    {
        AudioManager auidoManager = FindAnyObjectByType<AudioManager>();

        if (auidoManager.musicPlaying)
        {
            music_slider.value = 0;
            auidoManager.audioSource_music.mute = true;
            auidoManager.musicPlaying = false;
        }
        else
        {
            music_slider.value = 1;
            auidoManager.audioSource_music.mute = false;
            auidoManager.musicPlaying = true;
        }
        
    }


    [SerializeField] private Slider sound_slider;
    public void SoundBtn()
    {
        AudioManager auidoManager = FindAnyObjectByType<AudioManager>();

        if (auidoManager.can_play_sound)
        {
            sound_slider.value = 0;
            auidoManager.can_play_sound = false;
        }
        else
        {
            sound_slider.value = 1;
            auidoManager.can_play_sound = true;
        }

    }


    [SerializeField] private Slider Vibrate_slider;
    public void VibrateBtn()
    {
        AudioManager auidoManager = FindAnyObjectByType<AudioManager>();

        if (auidoManager.can_vibrate)
        {
            sound_slider.value = 0;
            auidoManager.can_vibrate = false;
        }
        else
        {
            sound_slider.value = 1;
            auidoManager.can_vibrate = true;
        }

    }

}
