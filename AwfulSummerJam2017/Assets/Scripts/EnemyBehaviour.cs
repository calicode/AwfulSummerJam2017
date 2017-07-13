using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float startSpeed = 10;
    public bool isActive;

    private float speed;
    private Rigidbody2D rb;

    void Start()
    {
        isActive = false;
        speed = startSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
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
        
}
