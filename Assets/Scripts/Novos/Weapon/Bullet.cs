using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    public Vector3 initialVelocity;
    public Vector3 initialPosition;
    public Vector3 bulletGravity;
    public float time;
    public float maxLifeTime;
    public TrailRenderer bulletTracer;

    public Bullet(Vector3 velocity, Vector3 position, TrailRenderer bulletTracer, float drop, float maxTime)
    {
        this.initialPosition = position;
        this.initialVelocity = velocity;
        this.time = 0f;
        this.bulletTracer = bulletTracer;
        this.bulletGravity = drop * Vector3.down;
        this.maxLifeTime = maxTime;
    }
        
}
