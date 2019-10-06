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

    public Transform[] targets;
    int index = 0;
    public bool ignoreY;
    void Start()
    {
        rBody = GetComponentInChildren<Rigidbody2D>();
        target = targets[index];
    }
    void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(bottomOffset.position, collisionRadius, groundLayer);

        input = Vector2.zero;

        Vector2 distFromTar = (target.position - rBody.transform.position);

        if (distFromTar.magnitude > 0.1f)
        {
            if (distFromTar.x < -0.01f)
            {
                input.x -= 1;
            }
            else if (distFromTar.x > 0.01f)
            {
                input.x += 1;
            }
            if (!ignoreY)
            {
                if (distFromTar.y < -0.01f)
                {
                    input.y -= 1;
                }
                else if (distFromTar.y > .01f)
                {
                    input.y += 1;
                }
            }

            if (input.x > 0 && !isFacingRight)
                FaceDirection(true);
            else if (input.x < 0 && isFacingRight)
                FaceDirection(false);

            input.Normalize();
            Walk(input);
        }
        else
        {
            index++;
            if (index >= targets.Length)
                index = 0;
            target = targets[index];
        }

    }
    public void Walk(Vector2 i)
    {
        i *= speed;
        if (ignoreY)
            rBody.velocity = new Vector2(i.x, rBody.velocity.y);
        else
            rBody.velocity = i;
    }
    void FaceDirection(bool right)
    {
        isFacingRight = right;
        image.flipX = !right;
    }
}


