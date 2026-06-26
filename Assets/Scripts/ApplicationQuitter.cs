using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplicationQuitter : MonoBehaviour
{

    [SerializeField] private GameObject LevelsMenu;
    [SerializeField] private Image DarkPanel;
    public void ExitTheApplication()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Application.Quit();
    }

    public void PlayBtn()
    {
        AudioManager.instance.PlayClip(1);
        LevelsMenu.SetActive(true);
    }

    public void BackBtn()
    {
        AudioManager.instance.PlayClip(1);
        LevelsMenu.SetActive(false);
    }

    public void OpenLevel(int num)
    {
        AudioManager.instance.PlayClip(1);
        PlayerPrefs.SetInt("Open Level", num);
        StartCoroutine(Opening());
    }

    IEnumerator Opening()
    {
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            Color c = DarkPanel.color;
            c.a = t;
            DarkPanel.color = c;
            yield return null;
        }
        SceneManager.LoadScene("Game Scene");
    }
}
