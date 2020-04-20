using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public int numberOfPlayers__;
    static int __numberOfPlayers;
    public static int numberOfPlayers { get { return __numberOfPlayers; } }

    public Color[] playerColors__;
    static Color[] __playerColors;
    
    public static Color GetColorOfPlayer(int ID)
    {
        if(ID >= __numberOfPlayers || ID < 0)
            return Color.white;

        return __playerColors[ID];
    }

    // Start is called before the first frame update
    void Start()
    {
        __numberOfPlayers = numberOfPlayers__;
        __playerColors = playerColors__;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
