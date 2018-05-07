using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInterfaceController_SC : MonoBehaviour
{
    public Text healthT;
    public Text scoreT;

    private PlayerController_SC playerSc;

    void Start ()
    {
        playerSc = FindObjectOfType<PlayerController_SC>();
        InterfaceUpdate();

        Dispatcher_SC.Subscribe(EventId.gameInterfaceNeedUpdate, OnGameInterfaceNeedUpdate);
	}

    private void OnDestroy()
    {
        Dispatcher_SC.Unsubscribe(EventId.gameInterfaceNeedUpdate, OnGameInterfaceNeedUpdate);
    }

    public void OnGameInterfaceNeedUpdate(EventInfo info)
    {
        InterfaceUpdate();
    }

    void InterfaceUpdate()
    {
        healthT.text = playerSc.health.ToString();
        scoreT.text = playerSc.score.ToString();
    }
}
