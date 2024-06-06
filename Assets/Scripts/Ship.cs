using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D collider;

    float thrust = 10.0f;
    float turnSpeed = 360.0f;   // 1 revolution per second
    const float moveSpeedMax = 10.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        // Translate
        Vector3 direction = transform.right;
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(direction * thrust);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-direction * thrust);
        }

        // Rotate
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0.0f, 0.0f, turnSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0.0f, 0.0f, -turnSpeed * Time.deltaTime);
        }

        // Shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        // Limit our movement speed (linear velocity) to a maximum
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, moveSpeedMax);

        // Wrap ship
        float size = Camera.main.orthographicSize;
        float aspect = Camera.main.aspect;
        float xMin = -size * aspect;
        float xMax =  size * aspect;
        float yMin = -size;
        float yMax =  size;
        float radius = collider.radius;

        if (transform.position.x < xMin)
        {
            transform.position = new Vector3(xMax - radius, transform.position.y);
        }
        if (transform.position.x > xMax)
        {
            transform.position = new Vector3(xMin + radius, transform.position.y);
        }
        if (transform.position.y < yMin)
        {
            transform.position = new Vector3(transform.position.x, yMax - radius);
        }
        if (transform.position.y > yMax)
        {
            transform.position = new Vector3(transform.position.x, yMin + radius);
        }

        Debug.DrawLine(transform.position, transform.position + direction * 5.0f, Color.red);
    }
}
