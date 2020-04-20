using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interceptor : Orbiter
{
    public new SpriteRenderer renderer;
    public TrailRenderer trail;

    void Start()
    {
        SetInMotion(transform.position, velocity__, playerID__);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (BattleTimeManager.isPaused)
            return;

        transform.position = trajectory.Next;

        transform.up = trajectory.Velocity;
    }
    
    public void SetInMotion(Vector2 initialPosition, Vector2 initialVelocity, int playerID)
    {
        trajectory = new Trajectory(initialPosition, initialVelocity);

        __playerID = playerID;

        SetColor(Players.GetColorOfPlayer(__playerID));
    }
    
    public override void SetColor(Color color)
    {
        renderer.color = color;
        
        color.r *= trailColorValue;
        color.g *= trailColorValue;
        color.b *= trailColorValue;
        trail.startColor = color;
        trail.endColor = color;
    }
}
