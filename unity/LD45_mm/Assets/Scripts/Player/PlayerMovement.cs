using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public float airControlSpeed;
    public float jumpSpeed = 5;
    Vector2 input;
    Rigidbody2D rBody;

    public float fallMult = 2.5f;
    public float lowJumpMult = 2.0f;

    public bool isGrounded = false;
    public bool wasGrounded;
    public LayerMask groundLayer;
    public float collisionRadius = 1;
    public Transform bottomOffset;
    public bool isFacingRight = true;
    public float jumpHoldTime = 1;
    float jumpHoldTimeTrack;
    ModuleSystem mSystem;
    bool holdingJump = true;

    public float coyoteTime = 0.1f;
    public float coyoteTimeTrack;
    public static PlayerMovement instance;

    public float recoilTime = 0.5f;
    float recoilTrack;
    bool isRecoiling;
    Vector2 recoilDir;

    public Animator anim;
    public ParticleSystem dust;
    public float timeScale = 1;
    public float TimeScale { get => Time.deltaTime * timeScale; }

    public bool canMove = true;
    public bool isSwinging = false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        mSystem = GetComponent<ModuleSystem>();
        SavedData.Load();
        foreach (Type t in SavedData.allTypes)
        {
            //mSystem.AddCollectedModule(t);
        }

        if (SavedData.allTypes.Count == 0) //Starting new game
        {
            //mSystem.AddCollectedModule<CoreModule>();
        }

        mSystem.AddCollectedModule<CoreModule>();
        mSystem.AddCollectedModule<WalkModule>();
        mSystem.AddCollectedModule<DoubleJumpModule>();
        mSystem.AddCollectedModule<GunModule>();
        mSystem.AddCollectedModule<MonochromeModule>();
        mSystem.AddCollectedModule<FullSightModule>();
        mSystem.AddCollectedModule<ChargeGunModule>();
        mSystem.AddCollectedModule<EnemyHealthModule>();
        mSystem.AddCollectedModule<DashModule>();
        mSystem.AddCollectedModule<ShieldModule>();


    }

    // Update is called once per frame
    void Update()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(bottomOffset.position, collisionRadius, groundLayer);
        anim.SetBool("isGrounded", isGrounded);
        if (wasGrounded && !isGrounded)
        {
            coyoteTimeTrack = Time.time + coyoteTime;
        }

        input = Vector2.zero;

        if (isRecoiling)
        {
            input = recoilDir;
            Recoil(input);
            if (Time.time > recoilTrack) isRecoiling = false;
        }
        else
        {
            if (canMove)
            {
                Debug.Log("Horizontal axis: " + Input.GetAxis("Horizontal"));
                input.x = Input.GetAxis("Horizontal");
                // input.x = Input.GetAxis("Aim");
            }
            if (input.x > 0 && !isFacingRight)
                FaceDirection(true);
            else if (input.x < 0 && isFacingRight)
                FaceDirection(false);

            if (mSystem.HasModule<WalkModule>())
                Walk(input);
            else
                Walk(input / 2);
        }

        if (!isGrounded)
        {
            holdingJump = jumpHoldTimeTrack > 0 && Input.GetKey(KeyCode.Space);
            jumpHoldTimeTrack -= TimeScale;

            if (rBody.velocity.y < 0)
            {
                rBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * TimeScale;
            }
            else if (rBody.velocity.y > 0 && !holdingJump)
            {
                jumpHoldTimeTrack -= TimeScale;
                rBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMult - 1) * TimeScale;
            }
        }
        anim.SetFloat("velocityY", rBody.velocity.y);
        SetDustEmmision();
    }

    public void Walk(Vector2 i)
    {
        anim.SetBool("Walk", (i.x != 0));
        i *= speed;
        rBody.velocity = new Vector2(i.x, rBody.velocity.y);
    }

    public void Recoil(Vector2 i)
    {
        i *= speed;
        rBody.velocity = new Vector2(i.x, 0);
    }

    void FaceDirection(bool right)
    {
        isFacingRight = right;
        transform.localEulerAngles = new Vector3(0, right ? 0 : 180, 0);
    }

    void SetDustEmmision()
    {
        EmissionModule em = dust.emission;
        em.enabled = isGrounded && Mathf.Abs(input.x) > 0;
    }

    public void Jump()
    {
        jumpHoldTimeTrack = jumpHoldTime;
        rBody.velocity = new Vector2(rBody.velocity.x, 0);
        rBody.velocity += Vector2.up * jumpSpeed;
        anim.SetTrigger("Jump");
        anim.SetBool("isGrounded", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(bottomOffset.position, collisionRadius);
    }

    public void HitPlayer(Transform source)
    {
        isRecoiling = true;
        Vector3 result;
        if (source.position.x > transform.position.x)
            result = Vector3.left;
        else
            result = Vector3.right;

        recoilDir = result;
        recoilTrack = Time.time + recoilTime;
    }

    public void Dash(float dashTime, float speed)
    {
        Debug.LogWarning("XXX");
        if (isRecoiling) return;
        isRecoiling = true;
        recoilDir = speed * (isFacingRight ? Vector3.right : Vector3.left);
        recoilTrack = Time.time + dashTime;
    }
}
