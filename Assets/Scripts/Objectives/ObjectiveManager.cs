using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectiveDefinition
{
    public ObjectiveType type;
    public string displayName = "Objective";
    public int targetAmount = 1;
}

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance {  get; private set; }

    [Tooltip("Objectives active")]
    public List<ObjectiveDefinition> objectiveDefinitions = new List<ObjectiveDefinition>();

    public IReadOnlyList<Objective> Objectives => _objectives;
    readonly List<Objective> _objectives = new List<Objective>();

    public event Action<Objective> OnObjectiveProgress;
    public event Action<Objective> OnObjectiveCompleted;
    public event Action OnObjectivesInitialized;
    public event Action OnAllObjectivesCompleted;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (ObjectiveDefinition def in objectiveDefinitions)
            _objectives.Add(new Objective(def.type, def.displayName, def.targetAmount));

        OnObjectivesInitialized?.Invoke();
    }

    public void ReportProgress(ObjectiveType type, int amount = 1)
    {
        Objective objective = _objectives.Find(o => o.type == type && !o.IsComplete);
        if (objective == null) return;

        objective.currentAmount = Mathf.Min(objective.currentAmount + amount, objective.targetAmount);
        OnObjectiveProgress?.Invoke(objective);

        if (objective.IsComplete)
        {
            OnObjectiveCompleted?.Invoke(objective);

            if (AllObjectivesComplete())
                OnAllObjectivesCompleted?.Invoke();
        }
    }

    public bool AllObjectivesComplete()
    {
        foreach (Objective o in _objectives)
            if (!o.IsComplete) return false;
        return true;
    }
}
