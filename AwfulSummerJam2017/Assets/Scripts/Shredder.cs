using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Shredded object: " + collider);
        Destroy(collider.gameObject);
    }
}
