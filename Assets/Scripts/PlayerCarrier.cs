using UnityEngine;

public class PlayerCarrier : MonoBehaviour
{
    public ICarryable CarriedItem { get; private set; }
    public bool IsCarrying => CarriedItem != null;

    //public FoodItem CarriedFood { get; private set; }
    public Transform holdPoint;
    public MeshFilter carryMeshFilter;
    public MeshRenderer carryMeshRenderer;

    public KeyCode dropKey = KeyCode.G;
    public Transform dropPoint;

    void Update()
    {
        if (IsCarrying && Input.GetKeyDown(dropKey))
            DropCarriedItem();
    }

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

        if (carryable is SkateItem && SkateReturnPoint.Instance != null)
            SkateReturnPoint.Instance.SetHighlight(true);
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
        bool wasSkate = CarriedItem is SkateItem;

        carryMeshRenderer.enabled = false;

        Transform itemTransform = CarriedItem.GameObject.transform;
        itemTransform.SetParent(null);
        itemTransform.position = dropPoint != null ? dropPoint.position : transform.position + transform.forward;
        itemTransform.rotation = Quaternion.identity;

        CarriedItem.OnDropped();
        CarriedItem = null;

        if (wasFood && TableManager.Instance != null)
            TableManager.Instance.ShowActiveTables(false);

        if (wasSkate && SkateReturnPoint.Instance != null)
            SkateReturnPoint.Instance.SetHighlight(false);
    }

    public void ReturnSkate(SkateReturnPoint point)
    {
        SkateItem skate = CarriedItem as SkateItem;
        if (skate == null) return;

        carryMeshRenderer.enabled = false;
        CarriedItem = null;

        point.SetHighlight(false);

        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.ReportProgress(ObjectiveType.ReturnSkates);

        Destroy(skate.gameObject);
    }
}