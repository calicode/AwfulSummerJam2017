using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour 
{
    
    void OnTriggerEnter2D(Collider2D col)
    {
        EnemyBehaviour enemy = col.gameObject.GetComponent<EnemyBehaviour>();

        if(enemy)
        {
            enemy.StartMoving();
            Debug.Log("Activating Enemy!"); //remove this before the game is done
        }
    }
}
