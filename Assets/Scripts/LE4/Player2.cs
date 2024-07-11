using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    Rigidbody2D rb;

    const float xMoveAcc = 15.0f;
    const float xSpeedMax = 10.0f;
    const float yJumpSpeed = 10.0f;

    const float groundDrag = 0.25f;
    const float airDrag = 0.5f;

    const int jumpCount = 2;
    int jumps = 0;

    bool grounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float dt = Time.deltaTime;
        rb.velocity += Vector2.right * xMoveAcc * Input.GetAxisRaw("Horizontal") * dt;

        if (Input.GetKeyDown(KeyCode.W) && jumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, yJumpSpeed);
            jumps--;
        }
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector2 acc = grounded ? Vector2.zero : Physics2D.gravity;
        rb.velocity += acc * dt;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -xSpeedMax, xSpeedMax), rb.velocity.y);
        rb.velocity *= Mathf.Pow(grounded ? groundDrag : airDrag, dt);
    }

    // Stop horizontal motion if x-collision, stop vertical motion if y-collision. If corner collision, maintain motion and resolve position.
    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];

        if (contact.normal.y == 0.0f)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
        else if (contact.normal.x == 0.0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
        }
        else
        {
            // Don't do anything -- preserve motion & resolve position only in collision-stay.
        }
    }

    // Move player along "collision normal" via "minimum translation vector" every frame of collision to resolve!
    void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        Vector2 mtv = contact.normal * Mathf.Abs(contact.separation);
        rb.position += mtv;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        grounded = true;
        jumps = jumpCount;
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
    }
}