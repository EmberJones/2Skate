using UnityEngine;

public class PlayerCarrier : MonoBehaviour
{
    public ICarryable CarriedItem { get; private set; }
    public bool IsCarrying => CarriedItem != null;

    //public FoodItem CarriedFood { get; private set; }
    public Transform holdPoint;
    public MeshFilter carryMeshFilter;
    public MeshRenderer carryMeshRenderer;

    void OnTriggerEnter(Collider other)
    {
        if (IsCarrying) return;

        ICarryable carryable = other.GetComponent<ICarryable>();
        if (carryable != null && carryable.CanBePickedUp) PickUp(carryable);
    }

    void PickUp(ICarryable carryable)
    {
        CarriedItem = carryable;

        MeshFilter itemMesh = carryable.MeshSource.GetComponentInChildren<MeshFilter>();
        MeshRenderer itemRenderer = carryable.MeshSource.GetComponentInChildren<MeshRenderer>();

        carryable.OnPickedUp();

        carryMeshFilter.sharedMesh = itemMesh.sharedMesh;
        carryMeshRenderer.sharedMaterials = itemRenderer.sharedMaterials;
        carryMeshRenderer.enabled = true;

        if (carryable is FoodItem && TableManager.Instance != null)
            TableManager.Instance.ShowActiveTables(true);
    }

    public void DeliverFood(Table table)
    {
        FoodItem food = CarriedItem as FoodItem;
        if (food == null) return; // not currently carrying food

        carryMeshRenderer.enabled = false;
        food.transform.SetParent(table.foodPlacementPoint);
        food.transform.localPosition = Vector3.zero;
        food.gameObject.layer = LayerMask.NameToLayer("Default");
        Rigidbody foodRb = food.GetComponent<Rigidbody>();
        if (foodRb != null) foodRb.isKinematic = true;
        food.isDelivered = true;
        food.gameObject.SetActive(true);
        CarriedItem = null;

        if (TableManager.Instance != null)
            TableManager.Instance.ShowActiveTables(false);

        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.ReportProgress(ObjectiveType.DeliverFood);
    }

    public void DropCarriedItem()
    {
        if (CarriedItem == null) return;

        bool wasFood = CarriedItem is FoodItem;

        carryMeshRenderer.enabled = false;
        CarriedItem.OnDropped();
        CarriedItem = null;

        if (wasFood && TableManager.Instance != null)
            TableManager.Instance.ShowActiveTables(false);
    }
}