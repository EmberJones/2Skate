using UnityEngine;

public class Lights : MonoBehaviour
{
    public float changeTime = 0.5f;

    //public float FadeSpeed = 5f;

    //public float brightness = 1f;


    public Light _light;
    public Color theeColour;
    private float timer;


    void Awake()
    {
        _light = GetComponent<Light>();
        PickNewColor();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeTime)
        {
            timer = 0f;
            PickNewColor();
        }

    }

    private void PickNewColor()
    {
        int num = Random.Range(0,6);
        ChangeColour(num);
        
    }

    public void ChangeColour(float num)
    {
        switch (num)
        {
            case 0:
                ColorUtility.TryParseHtmlString("#C30524", out theeColour); // red
                break;

            case 1:
                ColorUtility.TryParseHtmlString("#FF7400", out theeColour); // orange
                break;

            case 2:
                ColorUtility.TryParseHtmlString("#E9E500", out theeColour); // yellow
                break;

            case 3:
                ColorUtility.TryParseHtmlString("#00C830", out theeColour); // green
                break;

            case 4:
                ColorUtility.TryParseHtmlString("#0078FF", out theeColour); // blue
                break;

            case 5:
                ColorUtility.TryParseHtmlString("#5600FF", out theeColour); // purple
                break;


        }

        _light.color = theeColour;
    }
}

