using System;
using TMPro;
using UnityEngine;

public class OldStyleUIManager : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public bool red = true;
    public void UpdateTimer(int timer)
    {
        if (timer <= 0)
        {
            Debug.Log("0 Seconds");
            this.timer.text = FormatTime(0);
            GameEvents.RaiseGameOver();
            Time.timeScale = 0f;
        }
        else
        {
            if (timer <= 30)
            {
                Debug.Log("Change Colours");
                if (red)
                {
                    this.timer.color = Color.red;
                    //this.timer.text = FormatTime(0)
                    red = false;
                }
                else
                {
                    this.timer.color = Color.white;
                    //this.timer.text = Convert.ToString(timer);
                    red = true;
                }
            }

            this.timer.text = FormatTime(timer);
            Debug.Log("Couting Down");

        }
    }
    private string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes}:{seconds:D2}";
    }
}
