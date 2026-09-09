using UnityEngine;
using System;

[Serializable]
public class Objective
{
    public ObjectiveType type;
    public string displayName;
    public int currentAmount;
    public int targetAmount;

    public bool IsComplete => currentAmount >= targetAmount;

    public Objective(ObjectiveType type, string displayName, int targetAmount)
    {
        this.type = type;
        this.displayName = displayName;
        this.targetAmount = targetAmount <= 0 ? 1 : targetAmount;
        currentAmount = 0;
    }
}
