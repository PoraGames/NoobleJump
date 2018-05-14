using UnityEngine;
using System.Collections;

public class LoadSceneEventSender_SC : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Dispatcher_SC.Send(EventId.needLoadScene, new EventInfo(sceneName));
    }

    public void ReloadScene()
    {
        Dispatcher_SC.Send(EventId.needReloadScene, new EventInfo());
    }
}
