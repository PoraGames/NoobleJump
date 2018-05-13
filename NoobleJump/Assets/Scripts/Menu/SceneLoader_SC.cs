using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader_SC : MonoBehaviour
{
    public Image splashImage;
    public float timeBeforeStartSceneLoading = 1f;
    public float timeAfterSceneLoading = 1f;

    private string needLevelName;
    private AsyncOperation asyncOperation;

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void LoadScene(string sceneName)
    {
        // Dispatcher_SC.Send(EventId.splashOn, new EventInfo());
        needLevelName = sceneName;

        StartCoroutine(LoadLevelWithAsync());
    }

    IEnumerator LoadLevelWithAsync()
    {
        float timer = 0f;
        while (timer < timeBeforeStartSceneLoading)
        {
            timer += Time.deltaTime;
            splashImage.color = new Color(1, 1, 1, timer / timeBeforeStartSceneLoading);

            yield return null;
        }

        asyncOperation = SceneManager.LoadSceneAsync(needLevelName);
        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            Debug.Log(asyncOperation.progress);

            yield return new WaitForFixedUpdate();
        }

        timer = 0f;
        while (timer < timeAfterSceneLoading)
        {
            Debug.Log("afterparty");
            timer += Time.deltaTime;
            splashImage.color = new Color(1, 1, 1, 1 - (timer / timeAfterSceneLoading));

            yield return null;
        }
    }
}
