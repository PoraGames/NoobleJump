using UnityEngine;
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
            if (Physics2D.OverlapCircle(point.position, groundCheckRadius, whatIsGround))
            {
                isGrounded = true;
                return isGrounded;
            }
        }

        isGrounded = false;
        return isGrounded;
    }
}
