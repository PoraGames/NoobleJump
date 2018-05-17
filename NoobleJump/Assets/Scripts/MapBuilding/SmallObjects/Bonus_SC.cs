using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BonusType
{
    empty,
    health,
    score,
}

public class Bonus_SC : MonoBehaviour
{
    public BonusType type = BonusType.empty;
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ищем игрока в вошедшем коллайдере
        PlayerController_SC playerSc = other.transform.GetComponent<PlayerController_SC>();

        // Если это игрок, то вызвать у него метод обработки бонусов
        if (playerSc)
        {
            if (type == BonusType.health)
            {
                playerSc.AddHealth(value);
            }
            if (type == BonusType.score)
            {
                playerSc.AddScore(value);
            }
            Destroy(gameObject);
        }
    }
}
