using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        mSystem = GetComponent<ModuleSystem>();
        mSystem.AddCollectedModule<CoreModule>(true);
        mSystem.AddCollectedModule<WalkModule>(true);
        mSystem.AddCollectedModule<JumpModule>(false);
        mSystem.AddCollectedModule<GunModule>(false);
        mSystem.AddCollectedModule<MonochromeModule>(true);
        mSystem.AddCollectedModule<FullSightModule>(false);
        mSystem.AddCollectedModule<ChargeGunModule>(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(bottomOffset.position, collisionRadius, groundLayer);

        if (wasGrounded && !isGrounded)
        {
            coyoteTimeTrack = Time.time + coyoteTime;
        }

        input = Vector2.zero;
        if (mSystem.HasModule<WalkModule>())
        {
            if (Input.GetKey(KeyCode.A))
            {
                input.x -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                input.x += 1;
            }

            if (input.x > 0 && !isFacingRight)
                FaceDirection(true);
            else if (input.x < 0 && isFacingRight)
                FaceDirection(false);

            Walk(input);
        }

        if ((isGrounded || Time.time < coyoteTimeTrack) && Input.GetKeyDown(KeyCode.Space) && mSystem.HasModule<JumpModule>())
        {
            Jump();
        }


        if (!isGrounded)
        {
            holdingJump = jumpHoldTimeTrack > 0 && Input.GetKey(KeyCode.Space);
            jumpHoldTimeTrack -= Time.deltaTime;

            if (rBody.velocity.y < 0)
            {
                rBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime;
            }
            else if (rBody.velocity.y > 0 && !holdingJump)
            {
                jumpHoldTimeTrack -= Time.deltaTime;
                rBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMult - 1) * Time.deltaTime;
            }
        }

    }

    public void Walk(Vector2 i)
    {
        i *= speed;
        rBody.velocity = new Vector2(i.x, rBody.velocity.y);
    }
    void FaceDirection(bool right)
    {
        isFacingRight = right;
        transform.localEulerAngles = new Vector3(0, right ? 0 : 180, 0);
    }
    void Run()
    {

    }

    void Jump()
    {
        jumpHoldTimeTrack = jumpHoldTime;
        rBody.velocity = new Vector2(rBody.velocity.x, 0);
        rBody.velocity += Vector2.up * jumpSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(bottomOffset.position, collisionRadius);
    }
}
