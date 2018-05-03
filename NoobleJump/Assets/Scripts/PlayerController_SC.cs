using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_SC : Unit_SC
{
    public float powerJump = 5f;
    public float horisontalSpeed = 2f;

    private float lastJumpTimer = 0f;
    private bool inJump = false;
    private Rigidbody2D rb;
    private Animator anim;
    private MapBuilder_SC mainMapBuilderSc;

    #region Unity selection
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mainMapBuilderSc = FindObjectOfType<MapBuilder_SC>();
    }

    void Update()
    {
        if (lastJumpTimer <= 9999f)// Во избежании переполнения (просто подстраховка)
            lastJumpTimer += Time.deltaTime;

        InputReader();
    }

    void FixedUpdate()
    {
        if (GroundCheck() && rb.velocity.y <= 0.01f && !inJump && lastJumpTimer >= 0.2f)
        {
            inJump = true;
            anim.Play("Jump");
        }

        // Если уже нет земли под ногами, а прыжок заряжается -> сбросить анимацию прыжка
        if (!isGrounded && inJump)
        {
            inJump = false;
            anim.Play("idle");

            Debug.Log("jump reset");
        }
    }
    #endregion

    /// <summary>
    /// Вся обработка входных данных
    /// </summary>
    private void InputReader()
    {
        float _coeff = 0;

        if (!inJump)
        {
            if (Input.GetKey(KeyCode.D))
                _coeff += 1;
            if (Input.GetKey(KeyCode.A))
                _coeff -= 1;
        }

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

    public void Jump()
    {
        inJump = false;
        lastJumpTimer = 0f;
        rb.velocity = new Vector2(rb.velocity.x, powerJump);
    }

    public override void Kill()
    {
        mainMapBuilderSc.RespawnPlayer();
    }
}
