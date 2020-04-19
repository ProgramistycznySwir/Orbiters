using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GravityGenerator : MonoBehaviour
{
    // Later this will have to be changed if stelar bodies would be able to move
    Vector2 position__;
    public Vector2 position { get {return position__; } }

    public bool massFromSize = true;

    public float mass = 1;

    void Awake()
    {
        position__ = transform.position;

        if(massFromSize)
            mass = Mathf.PI * transform.localScale.x * transform.localScale.x / 4f;

        Gravity.AddGenerator(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}