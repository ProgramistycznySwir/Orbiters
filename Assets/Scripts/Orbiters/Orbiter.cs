﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    public const float trailColorValue = 0.5f;

    public int playerID__;
    protected int __playerID;
    public int playerID { get { return __playerID; } }

    public Trajectory trajectory;

    public Vector2 velocity__;

    public LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = BattleTimeManager.frameRate * BattleTimeManager.turnFrames * 10 + 1;

        SetInMotion(transform.position, velocity__);

        if(line != null)
            line.SetPositions(trajectory.GetTrajectoryPositions3D(10, transform.position));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (BattleTimeManager.isPaused)
            return;

        transform.position = trajectory.Next;

        transform.up = trajectory.Velocity;
    }
    
    public void SetInMotion(Vector2 initialPosition, Vector2 initialVelocity)
    {
        trajectory = new Trajectory(initialPosition, initialVelocity);
    }
    
    public virtual void SetColor(Color color)
    {
        Debug.Log("Just dummy");
    }
    

    public static TrajectorySegment CalculateTrajectorySegment(Vector2 position, Vector2 velocity)
    {
        TrajectorySegment result = new TrajectorySegment();
        result.positions[0] = position + velocity / BattleTimeManager.frameRate;

        for(int i = 1; i < TrajectorySegment.lenght; i++)
        {
            velocity += Gravity.SampleGravityField(result.positions[i - 1]) / BattleTimeManager.frameRate;
            result.positions[i] = result.positions[i - 1] + velocity / BattleTimeManager.frameRate;
        }
        
        result.endVelocity = velocity;

        return result;
    }
}



public class Trajectory
{
    List<TrajectorySegment> segments = new List<TrajectorySegment>();
    
    public TrajectorySegment firstSegment {get { return segments[0]; } }
    public TrajectorySegment lastSegment {get { return segments[segments.Count - 1]; } }
    
    
    public Vector2 Next
    {
        get
        {
            if(firstSegment.HasEndedTransition)
                Step();
            
            return firstSegment.Next;
        }
    }
    
    public Vector2 Velocity { get { return firstSegment.Velocity; } }
    
    
    public Trajectory(Vector2 initialPosition, Vector2 initialVelocity)
    {
        segments.Add(Orbiter.CalculateTrajectorySegment(initialPosition, initialVelocity));
    }
    
    ///<summary>
    /// Calculates another step of trajectory
    ///</summary>
    public void Step()
    {
        // Robi to na zasadzie:
        // 1. Wzięcia elementu z początku listy
        // 2. Obliczenia dla niego nowych wartości (odświeżenia)
        // 3. Dodanie go na koniec listy
        
        
        TrajectorySegment segmentToRefresh = firstSegment;
        
        segmentToRefresh = Orbiter.CalculateTrajectorySegment(segments[segments.Count - 1].lastPosition, segments[segments.Count - 1].endVelocity);
        
        segments.RemoveAt(0);
        
        segments.Add(segmentToRefresh);
    }
    
    public List<TrajectorySegment> GetTrajectory(int lenght)
    {
        while(segments.Count < lenght)
            segments.Add(Orbiter.CalculateTrajectorySegment(lastSegment.lastPosition, lastSegment.endVelocity));
        
        return segments;
    }
    ///<summary>
    /// Gives only (and all) trajectory what was pre-calculated (probably not needed)
    /// Pass lenght of trajectory to get trajectory with desired lenght
    ///</summary>
    public List<TrajectorySegment> GetTrajectory()
    {
        return segments;
    }
    
    ///<summary>
    /// Gives only the trajectory segments which are specified by argument
    ///</summary>
    public TrajectorySegment[] GetTrajectorySegments(int lenght)
    {
        TrajectorySegment[] result = new TrajectorySegment[lenght];
        
        GetTrajectory(lenght);
        
        for (int i = 0; i < lenght; i++)
            result[i] = segments[i];
        
        return result;
    }
    
    public Vector2[] GetTrajectoryPositions(int lenght)
    {
        // Ensures trajectory has enough lenght
        GetTrajectory(lenght);
        
        Vector2[] positions = new Vector2[lenght * TrajectorySegment.lenght];
        
        int i = 0;
        
        for (int a = 0; a < lenght; a++)
        {
            for (int b = 0; b < TrajectorySegment.lenght; b++)
            {
                positions[i++] = segments[a].positions[b];
            }
        }
        
        return positions;
    }
    
    public Vector2[] GetTrajectoryPositions(int lenght, Vector2 firstPosition)
    {
        // Ensures trajectory has enough lenght
        GetTrajectory(lenght);
        
        Vector2[] positions = new Vector2[lenght * TrajectorySegment.lenght + 1];
        
        int i = 1;
        positions[0] = firstPosition;
        
        for (int a = 0; a < lenght; a++)
        {
            for (int b = 0; b < TrajectorySegment.lenght; b++)
            {
                positions[i++] = segments[a].positions[b];
            }
        }
        
        return positions;
    }
    
    public Vector3[] GetTrajectoryPositions3D(int lenght, Vector3 firstPosition)
    {
        // Ensures trajectory has enough lenght
        GetTrajectory(lenght);
        
        Vector3[] positions = new Vector3[lenght * TrajectorySegment.lenght + 1];
        
        int i = 1;
        positions[0] = firstPosition;
        
        for (int a = 0; a < lenght; a++)
        {
            for (int b = 0; b < TrajectorySegment.lenght; b++)
            {
                positions[i++] = segments[a].positions[b];
            }
        }
        
        return positions;
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
    public const int lenght = BattleTimeManager.frameRate * BattleTimeManager.turnFrames; //15 seconds since one seconds is 48 ticks
    
    public Vector2[] positions = new Vector2[lenght];
    public Vector2 lastPosition { get { return positions[lenght-1]; }}
    
    // This element could be calculated everytime but 8 bytes is not much space :)
    public Vector2 endVelocity;
    
    
    public bool HasEndedTransition { get { return (nextIndex >= lenght ? true : false); } }
    
    int nextIndex = 0;
    
    public Vector2 Next { get { return positions[nextIndex++]; } }
    
    public void Reset() // <Probably Unused>
    {
        nextIndex = 0;
    }
    
    public Vector2 Velocity
    {
        get
        {
            if(nextIndex >= lenght - 1)
                return endVelocity;
            return positions[nextIndex + 1] - positions[nextIndex]; 
        }
    }
}
