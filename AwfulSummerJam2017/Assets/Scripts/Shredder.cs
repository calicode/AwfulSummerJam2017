using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour 
{

    private EnemyBehaviour enemy;

    void OnTriggerEnter2D(Collider2D collider)
    {
        enemy = collider.gameObject.GetComponent<EnemyBehaviour>();

        if(enemy)
        {
            enemy.ResetPosition();    
        }

        if(collider.tag == "Ground")
        {
            collider.transform.parent.gameObject.SetActive(false);
        }
    }
}
