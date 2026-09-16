using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkateItem : CarryableItem
{
    public bool isReturned = false;
    public static List<SkateItem> Available = new List<SkateItem>();
    Vector3 _spawnPosition;
    Quaternion _spawnRotation;

    public override bool CanBePickedUp => !isReturned;

    void Awake()
    {
        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation;
    }

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
    public override void OnDropped()
    {
        // Reappear back at its rack so it's always findable again.
        transform.SetPositionAndRotation(_spawnPosition, _spawnRotation);
        gameObject.SetActive(true);
        outline.enabled = true;
    }

    void OnDisable()
    {
        Available.Remove(this);
    }
}
