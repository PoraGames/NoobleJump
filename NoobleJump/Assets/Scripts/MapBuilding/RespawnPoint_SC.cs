using UnityEngine;
using System.Collections;

public class RespawnPoint_SC : MonoBehaviour
{
    public Transform pointForRespawn;
    public Sprite onSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ищем игрока в вошедшем коллайдере
        PlayerController_SC playerSc = other.transform.GetComponent<PlayerController_SC>();

        // Если это игрок, то активировать точку
        if (playerSc)
        {
            Activate();
        }
    }

    public void Activate()
    {
        Dispatcher_SC.Send(EventId.newRespawnPoint, new EventInfo(pointForRespawn.position));
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (!sr)
            sr = GetComponentInChildren<SpriteRenderer>();
        if (sr)
            sr.sprite = onSprite;
    }
}
