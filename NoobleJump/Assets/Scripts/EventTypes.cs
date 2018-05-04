using UnityEngine;
using System.Collections;

public enum EventId
{
    unknown = 0,

    playerKilled,
}

public class EventInfo
{
    public string name;
    public int firstValue;

    public EventInfo(string _name, int _firstValue)
    {
        name = _name;
        firstValue = _firstValue;
    }
}

public class EventTypes : MonoBehaviour
{
}
