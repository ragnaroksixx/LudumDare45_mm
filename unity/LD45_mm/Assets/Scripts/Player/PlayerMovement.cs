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

    public bool onGround = false;
    public LayerMask groundLayer;
    public float collisionRadius = 1;
    public Transform bottomOffset;
    public bool isFacingRight = true;
    public float jumpHoldTime = 1;
    float jumpHoldTimeTrack;
    ModuleSystem mSystem;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        mSystem = GetComponent<ModuleSystem>();
        mSystem.ActivateModule<WalkModule>();
        mSystem.ActivateModule<JumpModule>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle(bottomOffset.position, collisionRadius, groundLayer);

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

        if (mSystem.HasModule<JumpModule>())
        {
            if (onGround && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }


        if (!onGround)
        {
            if (rBody.velocity.y < 0)
            {
                rBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime;
            }
            else if (rBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && jumpHoldTimeTrack > 0)
            {
                jumpHoldTimeTrack -= Time.deltaTime;
                rBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMult - 1) * Time.deltaTime;
            }
        }

    }

    void Walk(Vector2 i)
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
