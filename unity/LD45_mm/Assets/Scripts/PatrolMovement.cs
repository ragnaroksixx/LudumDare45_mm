using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class PatrolMovement : MonoBehaviour
{
    public float speed = 5;
    Vector2 input;
    Rigidbody2D rBody;
    public bool isGrounded = false;
    public LayerMask groundLayer;
    public float collisionRadius = 1;
    public Transform bottomOffset;
    public bool isFacingRight = true;
    Transform target;
    public SpriteRenderer image;

    public Transform targetA, targetB;

    void Start()
    {
        rBody = GetComponentInChildren<Rigidbody2D>();
        target = targetA;
    }
    void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(bottomOffset.position, collisionRadius, groundLayer);

        input = Vector2.zero;

        float distFromTar = target.position.x - rBody.transform.position.x;

        if (Mathf.Abs(distFromTar) > 0.1f)
        {
            if (distFromTar < 0)
            {
                input.x -= 1;
            }
            else
            {
                input.x += 1;
            }

            if (input.x > 0 && !isFacingRight)
                FaceDirection(true);
            else if (input.x < 0 && isFacingRight)
                FaceDirection(false);

            Walk(input);
        }
        else
        {
            if (target == targetA)
                target = targetB;
            else
                target = targetA;
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
        image.flipX = !right;
    }
}


