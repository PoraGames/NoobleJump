using UnityEngine;
using System.Collections;
using System;

public class Kill_SC : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit_SC _unitSc = collision.transform.GetComponent<Unit_SC>();
        if (_unitSc)
            _unitSc.Kill();
    }

    private void Awake()
    {
        Dispatcher_SC.Subscribe(EventId.playerKilled, ReactOnKill);
    }

    private void OnDestroy()
    {
        Dispatcher_SC.Unsubscribe(EventId.playerKilled, ReactOnKill);
    }

    public void ReactOnKill(EventId id, EventInfo info)
    {
        Debug.Log("ReactOnKill");
    }
}
