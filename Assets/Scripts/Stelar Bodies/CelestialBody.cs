using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CelestialBody : MonoBehaviour
{
    public enum OrbitType { CalculatedCircular, ForcedCircular, Realistic}
    
        [Tooltip("CalculatedCircular - Radial Velocity is overriden to make orbit circular in realistic manner\n" + 
            "ForcedCircular - orbit is circular no metter the Radial Velocity and how ridiculous it is\n" + 
            "Realistic - calculates orbit depending on specified Radial Velocity <Not Implemented>")]
    public OrbitType orbitType = OrbitType.CalculatedCircular;
    
        [Tooltip("If true body orbits in clockwise dirrection (if false, counterclockwise)")]
    public bool orbitClockwise;
    
    public float radialVelocity__;
    float __radialVelocity;
    public float radialVelocity { get { return __radialVelocity; } }
    
    // This is distance to parent <(for now there are only circular orbits implemented)>
    float __orbitRadius;
    public float orbitRadius { get { return __orbitRadius; } }
    
    public CelestialBodyTrajectory trajectory;
    
    void Awake()
    {
        __radialVelocity = radialVelocity__;
    }
    
    void Start()
    {
        if(orbitType == OrbitType.Realistic)
        {
                // <Not Implemented>
        }
        else
        {
            __orbitRadius = transform.localPosition.magnitude;
            if(orbitType == OrbitType.CalculatedCircular)
                CalculateOrbit();
            else if(orbitType == OrbitType.ForcedCircular)
            {
                // Nothing...
            }
        }

        trajectory = new CelestialBodyTrajectory(this);
    }

    Vector2 test;
    void FixedUpdate()
    {
        if(!trajectory.firstSegment.HasEndedTransition)
        {
            test = trajectory.firstSegment.Next;
            Debug.Log(test);
            transform.localPosition = test;
        }
    }
    
    // public Vector2 GetPosition(int index)
    // {
        
    // }

    public void CalculateOrbit()
    {
        float M = transform.parent.GetComponent<GravityGenerator>().mass;
        __radialVelocity = Mathf.Sqrt(Gravity.G * M / Mathf.Pow(orbitRadius, 3)) / 50; // 50 is a fixed frameRate (but like more fixed owo)
    }
    
    ///<summary>
    /// For only initialization of Celestial Body (non-static method)
    ///</summary>
    public TrajectorySegment CalculeteTrajectorySegment()
    //public TrajectorySegment CalculeteTrajectorySegment(Vector2 initialLocalPosition, float initialOrbitalVelocity) // <UNCOMPLETED>
    {
        TrajectorySegment result = new TrajectorySegment();

        // Angle in radians
        float orbitalAngle = Vector2.SignedAngle(Vector2.right, transform.localPosition) * Mathf.Deg2Rad;

        for(int i = 0; i < TrajectorySegment.lenght; i++)
        {
            orbitalAngle += radialVelocity;
            result.positions[i] = new Vector2(Mathf.Cos(orbitalAngle), Mathf.Sin(orbitalAngle)) * orbitRadius;
        }
        
        result.endOrbitalAngle = orbitalAngle;
        
        return result;
    }
}



public class CelestialBodyTrajectory : Trajectory
{
    public CelestialBodyTrajectory(CelestialBody celestialBody)
    {
        segments.Add(celestialBody.CalculeteTrajectorySegment());
    }
    
    public TrajectorySegment firstSegment
    {
        get
        {
            return segments[0];
        }
    }
}

// Za pomocą zrobienia CircularTrajectory wymusza się na obiekcie by był tam gdzie jego miejsce
public class CircularTrajectory : TrajectorySegment
{
    public new Vector2[] positions;
    
    public new Vector2 Next
    {
        get
        {
            if (++nextIndex >= positions.Length)
                nextIndex = 0;
            return positions[nextIndex];
        }
    }
}