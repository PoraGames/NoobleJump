using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController_SC : Unit_SC
{
    public float powerJump = 5f;
    public float horisontalSpeed = 2f;
    public Animator killAnim;
    public float score = 0;

    private Rigidbody2D rb;
    private Animator anim;
    private MapBuilder_SC mainMapBuilderSc;

    private float lastJumpTimer = 0f;
    private bool inJump = false;

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

        if (!inJump && !isUnderGameControl)
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
        killAnim.Play("kill");
        // Отнять 1 здоровье
        AddHealth(-1);
        Dispatcher_SC.Send(EventId.gameInterfaceNeedUpdate, new EventInfo());

        Invoke("RespawnPlayer", 1f);
        SetUnderGameControlState(true);

        Dispatcher_SC.Send(EventId.playerKilled, new EventInfo());
    }

    void RespawnPlayer()
    {
        mainMapBuilderSc.RespawnPlayer();
        killAnim.Play("idle");// Убрать эффект смерти с персонажа
        SetUnderGameControlState(false);
    }

    public override void SetUnderGameControlState(bool newState)
    {
        base.SetUnderGameControlState(newState);
        rb.velocity = Vector2.zero;
        rb.simulated = !newState;
    }

    public void AddHealth(int healthQuantity)
    {
        health += healthQuantity;
        Dispatcher_SC.Send(EventId.gameInterfaceNeedUpdate, new EventInfo());
    }

    public void AddScore(int scoreQuantity)
    {
        score += scoreQuantity;
        Dispatcher_SC.Send(EventId.gameInterfaceNeedUpdate, new EventInfo());
    }
}
