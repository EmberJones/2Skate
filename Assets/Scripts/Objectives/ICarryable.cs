using UnityEngine;

public interface ICarryable
{
    GameObject GameObject { get; }
    bool CanBePickedUp { get; }

    // Where PlayerCarrier pulls the mesh/material from to show while carried.
    Transform MeshSource { get; }

    void OnPickedUp();
    void OnDropped();
}