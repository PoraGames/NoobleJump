﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Все живое в игре
/// </summary>
public class Unit_SC : MonoBehaviour
{
    [Header("Unit")]
    public int health = 10;

    /// <summary>
    /// Находится на земле
    /// </summary>
    public bool isGrounded = false;

    /// <summary>
    /// Точки для проверки контакта с землей
    /// </summary>
    public Transform[] groundCheckPoints;

    public float groundCheckRadius = 0f;

    public LayerMask whatIsGround;

    public Platform_SC lastPlatform;

    /// <summary>
    /// Уничтожить этого юнита
    /// </summary>
    public virtual void Kill()
    {

    }

    protected bool GroundCheck()
    {
        foreach (Transform point in groundCheckPoints)
        {
            Collider2D _coll = Physics2D.OverlapCircle(point.position, groundCheckRadius, whatIsGround);
            if (_coll)
            {
                isGrounded = true;
                lastPlatform = _coll.transform.GetComponent<Platform_SC>();// Запоминаем платформу
                if (lastPlatform)
                    lastPlatform.OnPlayerJumpOnPlatform();// Сообщаем платформе о контакте
                return isGrounded;
            }
        }

        isGrounded = false;
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        foreach (Transform point in groundCheckPoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(point.position, groundCheckRadius);
        }
    }
}
