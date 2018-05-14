using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader_SC : MonoBehaviour
{
    public Image splashImage;
    public Slider progressBar;
    [Header("Время появления сплеша")]
    public float timeBeforeStartSceneLoading = 1f;
    [Header("Время исчезнавения сплеша")]
    public float timeAfterSceneLoading = 1f;
    [Header("Обязательное время загрузки")]
    public float minLoadinfTime = 2f;

    private string needLevelName;
    private AsyncOperation asyncOperation;
    public bool loadingNow = false;

    void Awake()
    {
        // При уровне High наблюдаются фризы
        // High = 50ms
        // Normal = 10ms
        // BelowNormal = 4ms
        // Low = 2ms
        Application.backgroundLoadingPriority = ThreadPriority.Normal;

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
    /// Асинхронная загрузка уровня, включает управление сплеш артом и прогресс баром
    /// </summary>
    IEnumerator LoadLevelWithAsync()
    {
        // Картинка перед загрузкой
        float timer = 0f;
        while (timer < timeBeforeStartSceneLoading)
        {
            timer += Time.deltaTime;
            splashImage.color = new Color(1, 1, 1, timer / timeBeforeStartSceneLoading);// Постепенное появление сплеш арта

            yield return null;
        }

        // Загрузка
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0f; // Сбросить прогресс прогресс бара
        asyncOperation = SceneManager.LoadSceneAsync(needLevelName); // Начать загружать на фоне
        float fakeTimeEnd = Time.time + minLoadinfTime; // Для симуляции загрузки сложного уровня
        while (!asyncOperation.isDone || Time.time < fakeTimeEnd)
        {
            // Прогресс бар двигается по самому медленному процессу "загрузки"
            float realProgress = asyncOperation.progress / 0.9f; // Реальный прогресс загрузки
            float fakeProgress = (minLoadinfTime - (fakeTimeEnd - Time.time)) / minLoadinfTime; // Симуляция
            if (fakeProgress < realProgress) // Берем наименьшее
                realProgress = fakeProgress;

            progressBar.value = realProgress; // Двигаем прогресс бар

            yield return null; // Ожидание до следующего Update;
        }

        // Картинка после загрузки
        progressBar.gameObject.SetActive(false);
        timer = 0f;
        while (timer < timeAfterSceneLoading)
        {
            timer += Time.deltaTime;
            splashImage.color = new Color(0.8f, 1, 0.8f, 1 - (timer / timeAfterSceneLoading));// Постепенное исчезание сплеш арта

            yield return null;
        }

        loadingNow = false;// Загрузка завершена
    }
}
