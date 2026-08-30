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

    void Awake() => _instance = this;
    public Table GetRandomTable()
    {
        if (tables.Count == 0) return null;
        return tables[Random.Range(0, tables.Count)];
    }
}