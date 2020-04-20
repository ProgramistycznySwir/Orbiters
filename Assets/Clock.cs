using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Clock : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public UnityEngine.UI.Image clockImage;
    
    // Update is called once per frame
    void Update()
    {
        timeText.text = $"Time: {BattleTimeManager.currentTime.ToString(TimeFormat.MonthsDays)}";
        clockImage.fillAmount = BattleTimeManager.currentTime.dateHours / 24f;
    }
}
