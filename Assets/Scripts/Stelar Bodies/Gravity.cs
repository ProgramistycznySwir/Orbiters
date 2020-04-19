using System.Collections;
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
    
    public static Vector2 SampleGravityField(Vector2 position)
    {
        Debug.Log($"Position: {position}");

        Vector2 acceleration = Vector2.zero;
        Vector2 direction;
        
        foreach(GravityGenerator generator in generators)
        {
            direction = generator.position - position;
            // if(direction.sqrMagnitude < 25f)
            //     continue;
            acceleration += direction.normalized * (generator.mass * G / direction.sqrMagnitude);

            Debug.Log($"One Generator: {direction.normalized * (generator.mass * G / direction.sqrMagnitude)}");
            // acceleration.x += direction.x * (generator.mass * G / direction.sqrMagnitude);
            // acceleration.y += direction.y * (generator.mass * G / direction.sqrMagnitude);
        }

        Debug.Log($"acceleration for {position} -> {acceleration}");

        return acceleration;
    }
    
    public static void AddGenerator(GravityGenerator generator)
    {
        generators.Push(generator);

        Debug.Log("BEEP BOOP LOGGED NEW GENERATOR OWO");
    }
}
