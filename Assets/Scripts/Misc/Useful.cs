using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class Useful
{
    public static Vector3 Vector2to3(Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}