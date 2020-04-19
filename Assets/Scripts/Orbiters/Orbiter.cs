using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Orbiter : MonoBehaviour
{
    [SerializeField]
    Vector2 __velocity;
    Vector2 position;
    public Vector2 velocity { get { return __velocity; } }

    public LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 5000;

        position = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(BattleTimeManager.isPaused)
            return;

        line.SetPositions(Gravity.CalculateTrajectoryForLineRenderer(position, velocity, 5000));

        __velocity += Gravity.SampleGravityField(transform.position) * Time.fixedDeltaTime;

        position += velocity * Time.fixedDeltaTime;

        transform.position = Useful.Vector2to3(position);


        transform.up = velocity;

        //line.SetPositions(Gravity.CalculateTrajectoryForLineRenderer(position, velocity, 5000));
    }

    // It's method instead of property to prevent form accidentally changing velocity when trying to get it
    public void SetVelocity(Vector2 velocity)
    {
        __velocity = velocity;
    }

    public static TrajectorySegment CalculateTrajectorySegment(Vector2 position, Vector2 velocity)
    {
        TrajectorySegment result = new TrajectorySegment();
        result.positions[0] = position + velocity;

        for(int i = 1; i < TrajectorySegment.lenght; i++)
        {
            velocity += Gravity.SampleGravityField(position);
            result.positions[i] = result.positions[i-1] + velocity * Time.deltaTime;
        }
        
        result.endVelocity = velocity;
        
        return result;
    }
}
