using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Clone Settings")]
    [SerializeField] private GameObject playerClonePrefab;
    [SerializeField] private Transform cloneSpawnPoint;

    private GameObject spawnedClone;
    private bool hasSpawnedClone = false;
    private bool isUnlockedByTarget = false;

    //Unlocks permission to spawn clone (called by TargetTrigger)
    public void UnlockClone() => isUnlockedByTarget = true;

    //Spawn clone if allowed and valid (only once, with 7 recorded moves)
    public void SummonPlayerClone(List<string> recordedMoves)
    {
        if (!isUnlockedByTarget || hasSpawnedClone || recordedMoves.Count != 7) return;

        spawnedClone = Instantiate(playerClonePrefab, cloneSpawnPoint.position, Quaternion.identity);
        hasSpawnedClone = true;

        if (spawnedClone.TryGetComponent(out PlayerClone cloneScript))
            cloneScript.SetReplayPath(recordedMoves);
        else
            Debug.LogWarning("⚠️ Clone prefab is missing PlayerClone script!");
    }
}
