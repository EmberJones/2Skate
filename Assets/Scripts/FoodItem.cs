using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public Table targetTable;
    public bool isDelivered = false;
    public static List<FoodItem> Available = new List<FoodItem>();
    public ParticleSystem indicatorPartilces;

    void OnEnable()
    {
        if (!isDelivered) 
        { 
            Available.Add(this); 
           
        }
        if (isDelivered)
        {
            indicatorPartilces.Stop();
        }
    }

    void OnDisable()
    {
        Available.Remove(this);
    }

    void Start()
    {
        if (targetTable == null && TableManager.Instance != null)
            targetTable = TableManager.Instance.GetRandomTable();
    }
}