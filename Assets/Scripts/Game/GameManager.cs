using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Clone Settings")]
    [SerializeField] private GameObject playerClonePrefab;
    [SerializeField] private Transform cloneSpawnPoint;

    private GameObject spawnedClone;
    private bool hasSpawnedClone = false;
<<<<<<< Updated upstream
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
            Debug.LogWarning("⚠️ Clone prefab thiếu script PlayerClone!");
=======

    // ✅ Giữ riêng quyền sinh clone cho TargetTrigger gọi
    public void TrySummonFromTarget(List<string> recordedMoves)
    {
        if (hasSpawnedClone || recordedMoves.Count != 7) return;

        spawnedClone = Instantiate(playerClonePrefab, cloneSpawnPoint.position, Quaternion.identity);
        hasSpawnedClone = true;

        PlayerClone cloneScript = spawnedClone.GetComponent<PlayerClone>();
        if (cloneScript != null)
        {
            cloneScript.SetReplayPath(recordedMoves);
        }
        else
        {
            Debug.LogWarning("⚠️ Clone prefab thiếu script PlayerClone!");
        }
>>>>>>> Stashed changes
    }
}
