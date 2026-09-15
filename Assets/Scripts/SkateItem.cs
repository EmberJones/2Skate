using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkateItem : CarryableItem
{
    public bool isReturned = false;
    public static List<SkateItem> Available = new List<SkateItem>();

    public override bool CanBePickedUp => !isReturned;

    void OnEnable()
    {
        if (!isReturned)
        {
            Available.Add(this);
        }
        else
        {
            if (outline != null) outline.enabled = false;
        }
    }

    void OnDisable()
    {
        Available.Remove(this);
    }
}
