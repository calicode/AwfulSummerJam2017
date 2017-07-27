﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingParticle : MonoBehaviour 
{
    public float speed = 15f;

    void FixedUpdate()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime); //Moves the platform left
    }
}
