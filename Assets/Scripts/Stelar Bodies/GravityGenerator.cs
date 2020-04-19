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
}