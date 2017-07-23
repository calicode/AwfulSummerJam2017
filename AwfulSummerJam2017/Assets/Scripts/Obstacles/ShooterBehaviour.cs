using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : EnemyBehaviour {

    public Transform gunBarrel;

    [SerializeField]
    private GameObject bullet;

    public void Shoot()
    {
        GameObject shot = Instantiate(bullet, gunBarrel.position, Quaternion.identity) as GameObject;
        shot.transform.parent = transform;
        Destroy(shot, 3f);
    }
	
}
