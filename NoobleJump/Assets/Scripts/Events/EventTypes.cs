using UnityEngine;
using System.Collections;

public enum EventId
{
    #region  empty info
    unknown = 0,

    playerKilled,
    playerRespawned,
    playerKilledAndHaveNotHealth,

    gamePaused,
    gameContinue,

    gameInterfaceNeedUpdate,
    #endregion

    newRespawnPoint,

    needLoadScene,
    needReloadScene,
}

public class EventInfo
{
    public string name = "unknown";
    public int firstValue = 0;
    public Vector2 position;
    public GameObject sender;

    public EventInfo() { }

    public EventInfo(Vector2 _position, GameObject _sender)
    {
        position = _position;
        sender = _sender;
    }

    public EventInfo(string _name)
    {
        name = _name;
    }
}

public class EventTypes : MonoBehaviour
{
}
