using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GravityGenerator : MonoBehaviour
{
    // Not needed hasCelestialBody or something like this, just go for celestialBody == null
    public CelestialBody celestialBody;
    
    // Later this will have to be changed if stelar bodies would be able to move
    Vector2 position__;
    public Vector2 position { get {return position__; } }

    public bool massFromSize = true;

    public float mass = 1;

    void Awake()
    {
        position__ = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(massFromSize)
            mass = Mathf.PI * transform.GetChild(0).localScale.x * transform.GetChild(0).localScale.x / 4f;
        
        Gravity.AddGenerator(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Vector2 Position(int t) // <Not Completed>
    {
        // Ta funkcja zwraca pozycję obiektu w świecie w czasie [teraz + t].
        // Robi to poprzez dodanie do siebie Position() każdego swojego rodzica
        // który też ma w sobie Celestial Body aż do pierwszego obiektu który nie ma skryptu CelestialBody.
        // (Tak włąściwie to jest to robione rekurencyjnie, fujka)
        // Te pozycje mogą być pre-kalkulowane jeszcze zobaczę czy żeby dokalkulowywał to dla każdego obiektu, czy tylko na żądanie
        return Vector2.zero;
    }
}