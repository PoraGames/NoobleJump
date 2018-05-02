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
    }

    void FixedUpdate()
    {
        if (GroundCheck() && rb.velocity.y <= 0.01f)
            Jump();
    }
    #endregion

    /// <summary>
    /// Вся обработка входных данных
    /// </summary>
    private void InputReader()
    {
        float _coeff = 0;
        if (Input.GetKey(KeyCode.D))
            _coeff += 1;
        if (Input.GetKey(KeyCode.A))
            _coeff -= 1;
        Move(_coeff);
    }

    private void Move(float moveCoeff)
    {
        // Отражение по осям (поворот)
        if (moveCoeff != 0)
            transform.localScale = new Vector3(1 * Mathf.Sign(moveCoeff), 1, 1);

        // Физика прыжка
        rb.velocity = new Vector2(horisontalSpeed * moveCoeff, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, powerJump);
    }
}
