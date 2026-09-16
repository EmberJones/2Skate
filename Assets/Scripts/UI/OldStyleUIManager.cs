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
            GameEvents.RaiseGameOver();
        }
        else 
        {
            if (timer <= 30)
            {
                Debug.Log("Change Colours");
                if (red)
                {
                    this.timer.color = Color.red;
                    this.timer.text = Convert.ToString(timer);
                    red = false;
                }
                else
                {
                    this.timer.color = Color.white;
                    this.timer.text = Convert.ToString(timer);
                    red = true;
                }
            }

            else { this.timer.text = Convert.ToString(timer); }
            Debug.Log("Couting Down");


        }

        
    }
}
