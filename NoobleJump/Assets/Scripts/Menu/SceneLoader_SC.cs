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
    private bool loadingNow = false;

    public void ReloadScene()
    {
        // Перезагрузка происходит достаточно быстро, судя по тестам
        // и пока не нуждается в асинхронной загрузке и сплешарте

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void LoadScene(string sceneName)
    {
        if (loadingNow)
        {
            Debug.LogError("Загрузка другого уровня еще не завершена");
            return;
        }

        loadingNow = true;
        needLevelName = sceneName;
        StartCoroutine(LoadLevelWithAsync());
    }

    /// <summary>
    /// Асинхронная загрузка уровня, включает управление сплеш артом
    /// </summary>
    IEnumerator LoadLevelWithAsync()
    {
        // Картинка перед загрузкой
        float timer = 0f;
        while (timer < timeBeforeStartSceneLoading)
        {
            timer += Time.deltaTime;
            splashImage.color = new Color(1, 1, 1, timer / timeBeforeStartSceneLoading);

            yield return null;
        }

        // Загрузка
        asyncOperation = SceneManager.LoadSceneAsync(needLevelName);
        asyncOperation.allowSceneActivation = true;
        while (!asyncOperation.isDone)
        {
            // TODO: Место для визуализации прогресса загрузки уровня

            yield return new WaitForFixedUpdate();
        }

        // Картинка после загрузки
        timer = 0f;
        while (timer < timeAfterSceneLoading)
        {
            Debug.Log("afterparty");
            timer += Time.deltaTime;
            splashImage.color = new Color(1, 1, 1, 1 - (timer / timeAfterSceneLoading));

            yield return null;
        }

        loadingNow = false;
    }
}
