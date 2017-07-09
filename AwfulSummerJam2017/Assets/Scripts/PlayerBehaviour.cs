using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float jumpForce;
    public bool isGrounded; //public for testing purposes, change to private once done
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Collider2D myCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        EnemyBehaviour enemy = col.gameObject.GetComponent<EnemyBehaviour>();

        if(enemy)
        {
            Debug.Log("Hells bells! I appear to have been hit!");
        }
    }

}
