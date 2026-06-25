using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        instance = this;
    }

    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject homePage;

    public void OpenSettingsPage()
    {
        settingsPage.SetActive(true);
    }

    public void OpenHomePage()
    {
        homePage.SetActive(true);
    }

}
