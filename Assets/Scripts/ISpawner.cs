using System.Collections;
using UnityEngine;

public interface ISpawner
{
    public bool IsSpawnAllow { get; set; }

    IEnumerator SpawnRoutine();
    void Spawn();
    void Destroyed();

    void Destroyed(GameObject item);
}
