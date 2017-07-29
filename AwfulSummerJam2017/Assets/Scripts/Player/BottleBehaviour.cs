using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleBehaviour : MonoBehaviour {

    public float speed;
    private Rigidbody2D rb; //The bottle's rigidbody
    private GameObject dustParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        transform.Rotate(Vector3.back, 360 * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Obstacle")
        {
            col.gameObject.SetActive(false);
        }
    }
}
