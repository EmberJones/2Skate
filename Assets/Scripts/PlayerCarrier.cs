using UnityEngine;

public class PlayerCarrier : MonoBehaviour
{
    public ICarryable CarriedItem { get; private set; }
    public bool IsCarrying => CarriedItem != null;

    public FoodItem CarriedFood { get; private set; }
    public Transform holdPoint;
    public MeshFilter carryMeshFilter;
    public MeshRenderer carryMeshRenderer;

    void OnTriggerEnter(Collider other)
    {
        if (CarriedFood != null) return;

        FoodItem food = other.GetComponent<FoodItem>();
        if (food != null &&!food.isDelivered) PickUp(food);
    }

    void PickUp(FoodItem food)
    {
        CarriedFood = food;
        food.gameObject.SetActive(false);

        MeshFilter foodMesh = food.GetComponentInChildren<MeshFilter>();
        MeshRenderer foodRenderer = food.GetComponentInChildren<MeshRenderer>();

        carryMeshFilter.sharedMesh = foodMesh.sharedMesh;
        carryMeshRenderer.sharedMaterials = foodRenderer.sharedMaterials;
        carryMeshRenderer.enabled = true;
    }

    public void DeliverFood(Table table)
    {

        carryMeshRenderer.enabled = false;
        CarriedFood.transform.SetParent(table.foodPlacementPoint);
        CarriedFood.transform.localPosition = Vector3.zero;
        CarriedFood.gameObject.layer = LayerMask.NameToLayer("Default");
        Rigidbody foodRb = CarriedFood.GetComponent<Rigidbody>();
        if (foodRb != null) foodRb.isKinematic = true;
        CarriedFood.isDelivered = true;
        CarriedFood.gameObject.SetActive(true);
        CarriedFood = null;

        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.ReportProgress(ObjectiveType.DeliverFood);
    }
}