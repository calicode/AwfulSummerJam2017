using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float startSpeed = 10;
    public bool isActive;

    private float speed;
    private Rigidbody2D rb;
    private Vector3 initialPosition;

    void Start()
    {
        isActive = false;
        speed = startSpeed;
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        if(!isActive)
        {
            speed = 0;
        }
        else
        {
            speed = startSpeed;
        }

        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }

    public void StartMoving()
    {
        isActive = true;
    }

    public void StopMoving()
    {
        isActive = false;
    }
       
    public void ResetPosition()
    {
        isActive = false;
        transform.localPosition = initialPosition;
    }
}
