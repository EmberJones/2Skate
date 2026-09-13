using UnityEngine;
using System.Collections.Generic;

public class TableManager : MonoBehaviour
{
    static TableManager _instance;
    public static TableManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<TableManager>();
            return _instance;
        }
    }
    public List<Table> tables = new List<Table>();
    private List<Table> activeTables = new List<Table>();

    void Awake() => _instance = this;

    void Start()
    {
        if (ObjectiveManager.Instance == null) return;

        Objective foodObjective = null;
        foreach (var o in ObjectiveManager.Instance.Objectives)
        {
            if (o.type == ObjectiveType.DeliverFood)
            {
                foodObjective = o;
                break;
            }
        }

        if (foodObjective != null)
        {
            ActivateTables(foodObjective.targetAmount);
        }

    }
    public void ActivateTables(int count)
    {
        foreach (var t in activeTables)
            t.SetActive(false);
        activeTables.Clear();

        List<Table> pool = new List<Table>(tables);
        count = Mathf.Min(count, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            Table chosen = pool[index];
            pool.RemoveAt(index);

            //chosen.SetActive(true);
            activeTables.Add(chosen);
        }
    }

    public bool IsTableActive(Table table) => activeTables.Contains(table);

    public void DeactivateTable(Table table)
    {
        if (activeTables.Remove(table))
            table.SetActive(false);
    }

    public Table GetRandomActiveTable()
    {
        if (activeTables.Count == 0) return null;

        int index = Random.Range(0, activeTables.Count);
        Table chosen = activeTables[index];
        activeTables.RemoveAt(index); // taken immediately, can't be re-assigned

        return chosen;
    }

    public Table GetRandomTable()
    {
        if (tables.Count == 0) return null;
        return tables[Random.Range(0, tables.Count)];
    }

    public void ShowActiveTables(bool show)
    {
        foreach (var t in activeTables)
            t.SetActive(show);
    }
}