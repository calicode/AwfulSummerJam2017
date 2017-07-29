using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : EnemyBehaviour {

    public Transform gunBarrel;

    private Animator anim;
    private SFXManager sfxManager;
    [SerializeField]
    private GameObject bullet;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        sfxManager = GameObject.FindObjectOfType<SFXManager>();
    }

    public void Shoot()
    {
        anim.SetTrigger("isShooting");
        ShotSFX();
        GameObject shot = Instantiate(bullet, gunBarrel.position, Quaternion.identity) as GameObject;
        shot.transform.parent = transform;
        Destroy(shot, 3f);
    }

    void ShotSFX()
    {
        sfxManager.audioSource[3].clip = sfxManager.shotSFX;
        sfxManager.audioSource[3].Play();
    }
	
}
