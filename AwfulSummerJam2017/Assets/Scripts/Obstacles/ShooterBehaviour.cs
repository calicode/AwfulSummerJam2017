using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : EnemyBehaviour {

    public Transform gunBarrel;
    private Animator anim;

    [SerializeField]
    private GameObject bullet;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public void Shoot()
    {
        anim.SetTrigger("isShooting");
        GameObject shot = Instantiate(bullet, gunBarrel.position, Quaternion.identity) as GameObject;
        shot.transform.parent = transform;
        Destroy(shot, 3f);
    }
	
}
