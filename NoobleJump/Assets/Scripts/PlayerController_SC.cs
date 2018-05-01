using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_SC : Unit_SC
{
    public float powerJump = 5f;
    public float horisontalSpeed = 2f;

    private Rigidbody2D rb;

    #region Unity selection
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputReader();

        if (GroundCheck())
            Jump();
    }
    #endregion

    /// <summary>
    /// Вся обработка входных данных
    /// </summary>
    private void InputReader()
    {

    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, powerJump);
    }
}
