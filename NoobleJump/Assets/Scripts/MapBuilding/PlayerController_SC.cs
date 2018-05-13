using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum MoveDirect
{
    none,
    left,
    right,
}

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

    public MoveDirect lastTapDirect = MoveDirect.none;

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

        InputReader();
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

#if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.D))
                lastTapDirect = MoveDirect.right;
            if (Input.GetKeyDown(KeyCode.A))
                lastTapDirect = MoveDirect.left;

            if (Input.GetKey(KeyCode.D) && (lastTapDirect == MoveDirect.right || _coeff == 0))
                _coeff = 1;
            if (Input.GetKey(KeyCode.A) && (lastTapDirect == MoveDirect.left || _coeff == 0))
                _coeff = -1;

#endif

#if UNITY_ANDROID

            if (Input.touchCount > 0)
            {
                MoveDirect touchDirect = MoveDirect.none;

                foreach (Touch touch in Input.touches)
                {
                    // Определение направления движения для этого касания
                    float xTouchPos = touch.position.x; // В пикселях
                    float xPosInScreenPart = xTouchPos / Screen.width; // В долях
                    if (xPosInScreenPart > 0.6f)
                        touchDirect = MoveDirect.right;
                    if (xPosInScreenPart < 0.4f)
                        touchDirect = MoveDirect.left;

                    // Если касание новое, то зарегистрировать актуальное направление
                    if (touch.phase == TouchPhase.Began)
                    {
                        lastTapDirect = touchDirect;
                    }

                    // Движение вправо
                    if (touchDirect == MoveDirect.right &&
                        lastTapDirect == MoveDirect.right ||
                        _coeff == 0 &&
                        touchDirect == MoveDirect.right)
                    {
                        _coeff = 1;
                    }

                    // Движение влево
                    if (touchDirect == MoveDirect.left &&
                        lastTapDirect == MoveDirect.left ||
                        _coeff == 0 &&
                        touchDirect == MoveDirect.left)
                    {
                        _coeff = -1;
                    }
                }
            }

#endif
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
        if (!inJump)// Предотвращение случайных вызовов
            return;

        // Если у платформы необычный коэффициент прыжка, то учесть его
        float powerJumpWithCoeffs = 0;
        if (lastPlatform)
        {
            powerJumpWithCoeffs = powerJump * lastPlatform.jumpCoeff;
        }

        inJump = false;
        lastJumpTimer = 0f;
        rb.velocity = new Vector2(rb.velocity.x, powerJumpWithCoeffs);
    }

    public override void Kill()
    {
        killAnim.Play("kill");
        // Отнять 1 здоровье
        AddHealth(-1);
        Dispatcher_SC.Send(EventId.gameInterfaceNeedUpdate, new EventInfo());
        Dispatcher_SC.Send(EventId.playerKilled, new EventInfo());

        // Если еще остались жизни, то автоматически возродить и продолжить игру,
        // если нет сообщить об этом всем
        if (health > 0)
        {
            Invoke("RespawnPlayer", 1f);
            SetUnderGameControlState(true);
        }
        else
        {
            Dispatcher_SC.Send(EventId.playerKilledAndHaveNotHealth, new EventInfo());
        }
    }

    void RespawnPlayer()
    {
        Dispatcher_SC.Send(EventId.playerRespawned, new EventInfo());
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
