using System.Collections.Generic;
using UnityEngine;

public enum DirectionType
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class ComboMovementEnergySpawner : MonoBehaviour
{
    [Header("Prefab and Objects")]
    [SerializeField] private GameObject energyPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector2 spawnOffset = Vector2.up;

    public List<string> recordedMoves = new List<string>();
    private int shootCount = 0;

    private void Update()
    {
        if (shootCount >= 7) return;

        if (Input.GetKeyDown(KeyCode.W)) Spawn(DirectionType.Up);
        else if (Input.GetKeyDown(KeyCode.S)) Spawn(DirectionType.Down);
        else if (Input.GetKeyDown(KeyCode.A)) Spawn(DirectionType.Left);
        else if (Input.GetKeyDown(KeyCode.D)) Spawn(DirectionType.Right);
    }

    private void Spawn(DirectionType dir)
    {
        if (shootCount >= 7) return;

        Vector3 spawnPos = playerTransform.position + (Vector3)spawnOffset;
        GameObject go = Instantiate(energyPrefab, spawnPos, Quaternion.identity);

        if (go.TryGetComponent(out EnergyProjectile proj))
        {
            recordedMoves.Add(dir.ToString());
            proj.SetTarget(targetTransform);
            proj.SetComboData(recordedMoves, shootCount);
        }

        shootCount++;
    }
}
