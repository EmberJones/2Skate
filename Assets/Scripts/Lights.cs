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
            case 0f:
                theeColour = Color.red; 
            break;

            case 1f:
                theeColour = new Color(1f, 0.5f, 0f); //Orange
                break;

            case 2f:
                theeColour = Color.yellow;
                break;

            case 3f:
                theeColour = Color.green;
                break;

            case 4f:
                theeColour = Color.blue;
                break;

            case 5f:
                theeColour = new Color(0.5f, 0f, 0.5f);// Purple
                break;


        }

        _light.color = theeColour;
    }
}

