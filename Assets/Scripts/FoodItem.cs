using System.Collections.Generic;
using UnityEngine;

public class FoodItem : CarryableItem
{
    public Table targetTable;
    public bool isDelivered = false;
    public static List<FoodItem> Available = new List<FoodItem>();

    public ParticleSystem indicatorPartilces;

    public override bool CanBePickedUp => !isDelivered;

    void OnEnable()
    {
        if (!isDelivered)
        {
            Available.Add(this);
        }
        else
        {
            if (indicatorPartilces != null) indicatorPartilces.Stop();
            if (outline != null) outline.enabled = false;
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