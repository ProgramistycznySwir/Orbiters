﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Gravity : MonoBehaviour
{
    // Convention note:
    // __name - pole
    // name_ - zmienna tymczasowa
    // name__ - zmienna dla Unity (by była widoczna w inspectorze)
    //
    // propertyName - jeśli jest to jedynie podanie przesłoniętego pola
    // PropertyName - jeśli jest to jakaś formuła, prawie metoda
    
    public float G__;
    static float __G;
    public static float G {get {return __G;}}
    
    static Stack<GravityGenerator> generators = new Stack<GravityGenerator>();
    
    // Start is called before the first frame update
    void Awake()
    {
        __G = G__;
    }
    
    ///<summary>
    /// Index 0 is literally passed position
    ///</summary>
    public static Vector2[] CalculateTrajectory(Vector2 position, Vector2 velocity, int steps) // <DEPRECATED>
    {
        Vector2[] result = new Vector2[steps + 1];
        result[0] = position;
        
        for(int i = 0; i < steps; i++)
        {
            result[i+1] = result[i] + velocity * Time.deltaTime;
            velocity += SampleGravityField(position);
        }
        
        return result;
    }
    ///<summary>
    /// Index 0 is literally passed position
    ///</summary>
    public static Vector3[] CalculateTrajectoryForLineRenderer(Vector2 position, Vector2 velocity, int steps) // <DEPRECATED>
    {
        Vector3[] result = new Vector3[steps + 1];
        result[0] = position;
        
        for(int i = 0; i < steps; i++)
        {
            result[i+1] = result[i] + Useful.Vector2to3(velocity) * Time.deltaTime;// * 5f;
            velocity += SampleGravityField(result[i]) * Time.fixedDeltaTime;// * 5f;
        }
        
        return result;
    }
    
    public static Vector2 SampleGravityField(Vector2 position)
    {
        Vector2 acceleration = Vector2.zero;
        Vector2 direction;
        
        foreach(GravityGenerator generator in generators)
        {
            direction = generator.position - position;
            // if(direction.sqrMagnitude < 25f)
            //     continue;
            acceleration += direction.normalized * (generator.mass * G / direction.sqrMagnitude);
            // acceleration.x += direction.x * (generator.mass * G / direction.sqrMagnitude);
            // acceleration.y += direction.y * (generator.mass * G / direction.sqrMagnitude);
        }
        
        return acceleration;
    }
    
    public static void AddGenerator(GravityGenerator generator)
    {
        generators.Push(generator);
    }
}
