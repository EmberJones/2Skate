using UnityEngine;

public class MopItem : CarryableItem
{
    Vector3 _spawnPosition;
    Quaternion _spawnRotation;

    void Awake()
    {
        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation;
    }

    public override void OnDropped()
    {
        // Reappear back at its rack so it's always findable again.
        transform.SetPositionAndRotation(_spawnPosition, _spawnRotation);
        gameObject.SetActive(true);
        outline.enabled = true;
    }
}
