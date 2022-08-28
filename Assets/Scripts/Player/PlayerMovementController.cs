using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public Animator animator;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float speedGain = 1;
    public float speedClamp = 20;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public int maxJump = 2;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;

    [Space] 
    public MMF_Player jumpEffect;
    public MMF_Player landEffect;

    [Space] public SpriteRenderer spriteRenderer;

    [Space]
    private PlayerCollision coll;
    private Rigidbody2D rb;
    private BetterJumping betterJumping;
    
    private bool groundTouch;
    private int jumpCount;

    private float acceleration;
    private float lastX;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<PlayerCollision>();
        rb = GetComponent<Rigidbody2D>();
        betterJumping = GetComponent<BetterJumping>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);

        if (Mathf.Abs(dir.x) > 0.1f) spriteRenderer.flipX = dir.x < 0;

        Walk(dir);

        if (coll.onWall && Input.GetButton("Fire3") && canMove)
        {
            wallGrab = true;
            wallSlide = false;
        }

        if (Input.GetButtonUp("Fire3") || !coll.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (coll.onGround)
        {
            wallJumped = false;
            betterJumping.enabled = true;
        }
        
        if (wallGrab)
        {
            rb.gravityScale = 0;
            if(x > .2f || x < -.2f) rb.velocity = new Vector2(rb.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        if(coll.onWall && !coll.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround)
            wallSlide = false;

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < maxJump)
            {
                Jump(Vector2.up, false);
                jumpCount++;
            }
            if (coll.onWall && !coll.onGround)
                WallJump();
        }

        if (coll.onGround && !groundTouch)
        {
            groundTouch = true;
            jumpCount = 0;
            landEffect.PlayFeedbacks();
        }

        if(!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }
        
        animator.SetBool("IsJumping", !coll.onGround);
        animator.SetBool("IsMoving", rb.velocity.magnitude > 0.1f);
        animator.SetBool("IsMovingX", Mathf.Abs(rb.velocity.x) > 0.1f );
        animator.SetBool("IsOnWall", coll.onWall);
        animator.SetBool("IsOnGround", coll.onGround);
        animator.SetFloat("SpeedX", rb.velocity.x);
        animator.SetFloat("SpeedY", rb.velocity.y);

        if (wallGrab || wallSlide || !canMove)
            return;

    }

    private void WallJump()
    {
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);
        
        animator.SetTrigger("WallJump");

        wallJumped = true;
    }

    private void WallSlide()
    {
        if (!canMove)
            return;

        bool pushingWall = (rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall);
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;
        
        if (coll.onGround && (dir.x > lastX || dir.x < lastX))
        {
            lastX = dir.x;
            acceleration = 0f;
        }

        acceleration += speedGain * Time.deltaTime;
        var finalSpeed = Mathf.Clamp(acceleration + speed, 0, speedClamp);

        if (!wallJumped) rb.velocity = new Vector2(dir.x * finalSpeed, rb.velocity.y);
        else rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * finalSpeed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
    }

    private void Jump(Vector2 dir, bool wall)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
        
        jumpEffect.PlayFeedbacks();
    }

    public void ResetMovement()
    {
        if (rb != null) rb.velocity = Vector2.zero;
        acceleration = 0f;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
