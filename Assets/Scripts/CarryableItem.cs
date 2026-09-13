using UnityEngine;

public abstract class CarryableItem : MonoBehaviour, ICarryable
{
    [SerializeField] protected Outline outline;

    public GameObject GameObject => gameObject;
    public Transform MeshSource => transform;
    public virtual bool CanBePickedUp => true;

    public virtual void OnPickedUp()
    {
        if (outline != null) outline.enabled = false;
        gameObject.SetActive(false);
    }

    public virtual void OnDropped()
    {

        gameObject.SetActive(true);
        //outline.enabled = true;
    }
}
