using UnityEngine;
using System.Collections;

public class RespawnPoint_SC : MonoBehaviour
{
    public Sprite onSprite;

    public void Activate()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (!sr)
            sr = GetComponentInChildren<SpriteRenderer>();
        if (sr)
            sr.sprite = onSprite;
    }
}
