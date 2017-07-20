using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour 
{
    private PlatformPooler platPool;
    private EnemyBehaviour enemy;

    void OnTriggerEnter2D(Collider2D collider)
    {
        enemy = collider.gameObject.GetComponent<EnemyBehaviour>();
        platPool = collider.gameObject.GetComponentInParent<PlatformPooler>();

        if(enemy)
        {
            enemy.ResetPosition();    
        }

        if(collider.tag == "Ground")
        {

            collider.transform.parent.gameObject.SetActive(false);
            platPool.AddPlatformBack(collider.transform.parent.gameObject);

        }

        if(collider.tag == "StartingPlatform")
        {
            collider.transform.parent.gameObject.SetActive(false); 
        }
    }
}
