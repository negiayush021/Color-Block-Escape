using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AsyncLoader : MonoBehaviour
{
    public Slider ProgessBar;
    public TextMeshProUGUI text_num;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        ProgessBar.value = 0;

        if (PlayerPrefs.GetInt("First Time") == 0)
        {
            StartCoroutine(AysncLoadScene(2));
            PlayerPrefs.SetInt("First Time", 1);
            return;
        }

        StartCoroutine(AysncLoadScene(1));

    }

    IEnumerator AysncLoadScene(int id)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id);
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;
        float targetProgress = 0f;

        while (!asyncLoad.isDone)
        {
            // The actual progress is from 0 to 0.9 before it jumps to 1
            targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Smoothly move progress bar towards target progress
            ProgessBar.value = Mathf.MoveTowards(ProgessBar.value, targetProgress, .5f * Time.deltaTime);

            // Update text as percentage
            int loading_num = Mathf.RoundToInt(ProgessBar.value * 100f);
            text_num.text = loading_num.ToString() + "%";

            // Optional: wait until fully loaded (progress >= 0.9)
            if (asyncLoad.progress >= 0.9f)
            {
                // Add a small artificial delay to show 100% filled bar
                timer += Time.deltaTime;
                if (timer >= 3f) // wait 1 second
                {
                    ProgessBar.value = 1f;
                    text_num.text = "100%";
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }

    }

}
