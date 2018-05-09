using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu_SC : MonoBehaviour
{
    public GameObject[] pauseMenuOgjects;
    public GameObject[] inGameMenuOgjects;

    public void PauseGame()
    {
        pauseMenuOgjects.SetActive(true);
        inGameMenuOgjects.SetActive(false);
        Dispatcher_SC.Send(EventId.gamePaused, new EventInfo());
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        pauseMenuOgjects.SetActive(false);
        inGameMenuOgjects.SetActive(true);
        Dispatcher_SC.Send(EventId.gameContinue, new EventInfo());
        Time.timeScale = 1;
    }
}
