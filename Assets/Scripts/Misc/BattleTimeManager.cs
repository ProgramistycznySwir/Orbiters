using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BattleTimeManager : MonoBehaviour
{
    public const int frameRate = 48;
    public const int turnFrames = 15;

    // Precalculated for speed
    static int __framesInTurn = frameRate * turnFrames;
    public static int framesInTurn {get { return __framesInTurn; } }

    static Ticks __currentTime;
    public static Ticks currentTime {get { return __currentTime; } }

    public static int turnTime { get { return currentTime.ticks % framesInTurn; } }

    static bool __isPaused;
    public static bool isPaused { get {return __isPaused; } }

    public KeyCode pauseKey = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        EndTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
            //__isPaused = !__isPaused;
            StartTurn();
    }
    
    void FixedUpdate()
    {
        //Debug.Log(currentTime.ticks);

        if (isPaused)
            return;

        __currentTime++;

        if (turnTime == 0)
            EndTurn();
    }
    
    public void StartTurn()
    {
        __isPaused = false;
        Time.timeScale = 1;
    }
    
    public void EndTurn()
    {
        __isPaused = true;
        Time.timeScale = 0;
    }
}



public struct TimeStamp
{
    Ticks __ticks;
    
    public int ticks {get { return __ticks.ticks; } }
    
    public Ticks age { get { return BattleTimeManager.currentTime - __ticks; } }
    
    public TimeStamp(Ticks time)
    {
        __ticks = time;
    }
    public TimeStamp(int time)
    {
        __ticks.ticks = time;
    }
    public TimeStamp(bool now)
    {
        __ticks = BattleTimeManager.currentTime;
    }
}

///<summary>
/// Hours(ticks)
/// Days
/// Months(turns)
/// Float (last element is float)
///</summary>
public enum TimeFormat { Hours, Days, DaysFloat, Months, MonthsFloat, DaysHours, MonthsDays, MonthsDaysFloat, MonthsDaysHours }

/// <summary>
/// Time and stuff
/// </summary>
public struct Ticks
{
    ///<value>
    ///Time in ticks (int)
    /// You can also set it if you want, just saying...
    ///</value>
    public int ticks;

    public int dateHours {get { return ticks % 24; } }
    
    ///<value>Time in days (float)</value>
    public float days {get { return (float)ticks / 24f; } }
    ///<value>Time in days (int)</value>
    public int wholeDays {get { return ticks / 24; } }
    ///<value>Time in months (int)</value>
    public int dateDays {get { return (ticks / 24) % 30; } }
    
    ///<value>Time in months (float)</value>
    public float months {get { return (float)ticks / 720f; } }
    ///<value>Time in months (int)</value>
    public int wholeMonths {get { return ticks / 720; } }


    public Ticks(int ticks)
    {
        this.ticks = ticks;
    }

    public string ToString(TimeFormat format)
    {
        if (ticks == 0)
            return "Now";

        switch(format)
        {
            case TimeFormat.Hours:
                return $"{ticks}h";
            case TimeFormat.Days:
                return $"{wholeDays}days";
            case TimeFormat.DaysFloat:
                return days.ToString("F1");
            case TimeFormat.Months:
                return $"{wholeMonths}months";
            case TimeFormat.MonthsFloat:
                return months.ToString("F1");
            case TimeFormat.DaysHours:
                return (wholeDays > 0 ? $"{wholeDays}days " : "") + $"{dateHours}h";
            case TimeFormat.MonthsDays:
                if (ticks < 24)
                    return $"{dateHours}h";
                return (wholeMonths > 0 ? $"{wholeMonths} month{(wholeMonths > 1 ? "s" : "")} " : "") + (dateDays > 0 ? $"{dateDays} day{(wholeDays > 1 ? "s" : "")}" : "");
            case TimeFormat.MonthsDaysFloat:
                string Df = ((ticks - wholeMonths * 720) / 24f).ToString("F1");
                return (wholeMonths > 0 ? $"{wholeMonths} month{(wholeMonths > 1 ? "s" : "")} " : "") + ((ticks - wholeMonths * 720) / 24f > 0 ? $"{Df} day{(wholeDays > 1 ? "s" : "")}" : "");
            case TimeFormat.MonthsDaysHours:
                return (wholeMonths > 0 ? $"{wholeMonths} month{(wholeMonths > 1 ? "s" : "")} " : "") + (dateDays > 0 ? $"{dateDays} day{(wholeDays > 1 ? "s" : "")} " : "") + $"{dateHours}h";
            default:
                return "[BŁĘDNY CZAS]";
        }
    }
    public override string ToString()
    { return $"{ticks}"; }

    public static Ticks operator +(Ticks a, Ticks b)
    {
        return new Ticks(a.ticks + b.ticks);
    }
    public static Ticks operator ++(Ticks a)
    {
        return new Ticks(++a.ticks);
    }
    public static Ticks operator -(Ticks a, Ticks b)
    {
        return new Ticks(a.ticks - b.ticks);
    }
}