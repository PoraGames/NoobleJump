using System.Collections;
using System.Collections.Generic;
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
    public bool loadingNow = false;

    void Awake()
    {
        Dispatcher_SC.Subscribe(EventId.needLoadScene, OnNeedLoadScene);
        Dispatcher_SC.Subscribe(EventId.needReloadScene, OnNeedReloadScene);
    }

    void OnDestroy()
    {
        Dispatcher_SC.Unsubscribe(EventId.needLoadScene, OnNeedLoadScene);
        Dispatcher_SC.Subscribe(EventId.needReloadScene, OnNeedReloadScene);
    }

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

        Time.timeScale = 1;
        loadingNow = true;
        needLevelName = sceneName;
        StartCoroutine(LoadLevelWithAsync());
    }

    void OnNeedReloadScene(EventInfo info)
    {
        ReloadScene();
    }

    void OnNeedLoadScene(EventInfo info)
    {
        LoadScene(info.name);
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
            splashImage.color = new Color(1, 1, 0, timer / timeBeforeStartSceneLoading);

            yield return null;
        }

        // Загрузка
        asyncOperation = SceneManager.LoadSceneAsync(needLevelName);
        while (!asyncOperation.isDone)
        {
            // TODO: Место для визуализации прогресса загрузки уровня
            splashImage.color = new Color(1 - asyncOperation.progress, 1, 0, 1);
            Debug.Log(asyncOperation.progress);

            yield return new WaitForFixedUpdate();
        }

        // Картинка после загрузки
        timer = 0f;
        while (timer < timeAfterSceneLoading)
        {
            timer += Time.deltaTime;
            splashImage.color = new Color(1, 1, 0, 1 - (timer / timeAfterSceneLoading));

            yield return null;
        }

        loadingNow = false;
    }
}
