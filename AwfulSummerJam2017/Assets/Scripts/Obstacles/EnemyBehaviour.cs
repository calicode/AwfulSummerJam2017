using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float startSpeed = 10; //Initial speed of the enemy, set to 0 for stationary baddies
    public bool isActive; //Is the enemy active? Not applicaple to stationary baddies (they don't actually care)

    private float speed; //Current speed of the enemy
    private Rigidbody2D rb; //The enemy's rigidbody
    private Vector3 initialPosition; //The enemy's starting position

    void Start()
    {
        //Makes sure the enemy is in position and waiting for the player
        isActive = false;
        speed = startSpeed;
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        //Sets the speed and motion of the enemy depending on if it's active or not
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

    //Activates the enemy
    public void StartMoving()
    {
        isActive = true;
    }

    //Deactivates the enemy (still deadly tho, just not moving)
    public void StopMoving()
    {
        isActive = false;
    }

    //Resets the enemy position and deactivates it
    public void ResetPosition()
    {
        StopMoving();
        transform.localPosition = initialPosition;
    }
}
