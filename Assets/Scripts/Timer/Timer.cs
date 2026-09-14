using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int time = 300;
    public OldStyleUIManager ui;
    
    public void StartTimer()
    {
        
            StartCoroutine(Countdown());
        

    }

    IEnumerator Countdown()
    {

    while (time >= 0)
    {
        Debug.Log("In Countdown");
        ui.UpdateTimer(time);
        time--;
        yield return new WaitForSeconds(1f);
    }
    }
}
