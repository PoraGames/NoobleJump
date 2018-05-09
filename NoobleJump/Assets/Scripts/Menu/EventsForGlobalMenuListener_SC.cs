using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsForGlobalMenuListener_SC : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject afterDeathMenu;

    void Start()
    {
        Dispatcher_SC.Subscribe(EventId.playerKilledAndHaveNotHealth, OnPlayerKilled);
    }

    void OnDestroy()
    {
        Dispatcher_SC.Unsubscribe(EventId.playerKilledAndHaveNotHealth, OnPlayerKilled);
    }

    void OnPlayerKilled(EventInfo info)
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(false);
        afterDeathMenu.SetActive(true);
    }
}
