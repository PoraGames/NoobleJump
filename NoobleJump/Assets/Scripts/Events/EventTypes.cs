using UnityEngine;
using System.Collections;

public enum EventId
{
    unknown = 0,

    playerKilled,
    playerRespawned,

    gamePaused,
    gameContinue,

    gameInterfaceNeedUpdate,
}

public class EventInfo
{
    public string name;
    public int firstValue;

    public EventInfo()
    {
        name = "unknown";
        firstValue = 0;
    }

    public EventInfo(string _name, int _firstValue)
    {
        name = _name;
        firstValue = _firstValue;
    }
}

public class EventTypes : MonoBehaviour
{
}
