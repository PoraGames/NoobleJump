using UnityEngine;
using System.Collections;

public enum EventId
{
    #region  empty info
    unknown = 0,

    playerKilled,
    playerRespawned,

    gamePaused,
    gameContinue,

    gameInterfaceNeedUpdate,
    #endregion

    newRespawnPoint,
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
}

public class EventTypes : MonoBehaviour
{
}
