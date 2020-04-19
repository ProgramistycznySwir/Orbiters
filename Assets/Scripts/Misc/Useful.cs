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



public class Trajectory
{
    protected List<TrajectorySegment> segments = new List<TrajectorySegment>();

    public Trajectory(Vector2 initialPosition, Vector2 initialVelocity)
    {
        segments.Add(Orbiter.CalculateTrajectorySegment(initialPosition, initialVelocity));
    }
    ///<summary>
    /// Just a paramaterless dummy so the children could derive
    ///</summary>
    public Trajectory()
    {
        
    }
    
    ///<summary>
    /// Calculates another step of trajectory
    ///</summary>
    public void Step()
    {
        // Robi to na zasadzie:
        // 1. Wzięcia elementu z początku listy
        // 2. Obliczenia dla niego nowych wartości
        // 3. Dodanie go na koniec listy
    }
    
    public List<TrajectorySegment> GetTrajectory(int lenght)
    {
        while(segments.Count < lenght)
            segments.Add(Orbiter.CalculateTrajectorySegment(segments[segments.Count-1].lastPosition, segments[segments.Count-1].endVelocity));
            
        return segments;
    }
    ///<summary>
    /// Gives only (and all) trajectory what was pre-calculated <(probably not needed)>
    /// Pass lenght of trajectory to get trajectory with desired lenght
    ///</summary>
    public List<TrajectorySegment> GetTrajectory()
    {
        return segments;
    }

    public TrajectorySegment GetSegment(int index)
    {
        return segments[index];
    }
}



///<summary>
/// Container for data related to trajectories
///</summary>
public class TrajectorySegment
{
    public const int lenght = 750; //15 seconds
    
    public Vector2[] positions = new Vector2[lenght];
    public Vector2 lastPosition { get { return positions[lenght-1]; }}
    
    // For Orbiters:
    public Vector2 endVelocity;
    // For Celestial Bodies:
    public float endOrbitalAngle; // Instead of lastPosition for Celestial Bodies
    
    
    public bool HasEndedTransition { get { return (nextIndex >= lenght ? true : false); } }

    protected int nextIndex = 0;
    
    public Vector2 Next { get { return positions[nextIndex++]; } }
    
    public void Reset() // <Probably Unused>
    {
        nextIndex = 0;
    }
}