using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ObjectiveListUI : MonoBehaviour
{
    [Serializable]
    public struct ObjectiveLabelBinding
    {
        public ObjectiveType type;
        public string labelName; // e.g. "listLabel_1"
    }

    public List<ObjectiveLabelBinding> labelBindings = new List<ObjectiveLabelBinding>
    {
        new ObjectiveLabelBinding { type = ObjectiveType.DeliverFood, labelName = "listLabel_1" },
        new ObjectiveLabelBinding { type = ObjectiveType.CleanBathroom, labelName = "listLabel_2" },
        new ObjectiveLabelBinding { type = ObjectiveType.PickUpTrash, labelName = "listLabel_3" },
        new ObjectiveLabelBinding { type = ObjectiveType.ReturnSkates, labelName = "listLabel_4" },
        new ObjectiveLabelBinding { type = ObjectiveType.KickOutDisruptiveCustomer, labelName = "listLabel_5" },
    };

    UIDocument _document;

    readonly Dictionary<ObjectiveType, Label> _labels = new Dictionary<ObjectiveType, Label>();

    void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        VisualElement root = _document.rootVisualElement;

        _labels.Clear();
        foreach (ObjectiveLabelBinding binding in labelBindings)
        {
            Label label = root.Q<Label>(binding.labelName);
            if (label != null)
                _labels[binding.type] = label;
            else
                Debug.LogWarning($"ObjectiveListUI: no label named '{binding.labelName}' found in the HUD UXML.");
        }

        if (ObjectiveManager.Instance != null)
            BindObjectives();
        else
            StartCoroutine(WaitForManager()); // handles Awake order between UI and manager
    }

    IEnumerator WaitForManager()
    {
        while (ObjectiveManager.Instance == null) yield return null;
        BindObjectives();
    }

    void BindObjectives()
    {
        // Set each bound label to its objective's current text immediately,
        // then let events keep it updated from here on.
        foreach (Objective objective in ObjectiveManager.Instance.Objectives)
        {
            if (_labels.TryGetValue(objective.type, out Label label))
                label.text = FormatText(objective);
        }

        ObjectiveManager.Instance.OnObjectiveProgress += HandleProgress;
        ObjectiveManager.Instance.OnObjectiveCompleted += HandleCompleted;
    }

    void HandleProgress(Objective objective)
    {
        if (_labels.TryGetValue(objective.type, out Label label))
            label.text = FormatText(objective);
    }

    void HandleCompleted(Objective objective)
    {
        if (_labels.TryGetValue(objective.type, out Label label))
            label.AddToClassList("objective-complete");
    }

    string FormatText(Objective objective) =>
        $"{objective.displayName}: {objective.currentAmount}/{objective.targetAmount}";

    void OnDisable()
    {
        if (ObjectiveManager.Instance == null) return;
        ObjectiveManager.Instance.OnObjectiveProgress -= HandleProgress;
        ObjectiveManager.Instance.OnObjectiveCompleted -= HandleCompleted;
    }
}
