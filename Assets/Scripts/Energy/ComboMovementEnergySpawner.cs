using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream

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

    private List<string> recordedMoves = new List<string>();
=======

public class ComboMovementEnergySpawner : MonoBehaviour
{
    [Header("Prefab và liên kết")]
    [SerializeField] private GameObject energyPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private int maxShootCount = 7;
    [SerializeField] private Vector2 spawnOffset = Vector2.up;

    private List<string> recordedMoves = new();
>>>>>>> Stashed changes
    private int shootCount = 0;
    private bool canReadInput = true;
    private string lastInput = "";

    //Check player input and spawn energy if shoot count < 7
    private void Update() 
    {
        if (shootCount >= 7) return;

<<<<<<< Updated upstream
        if (Input.GetKeyDown(KeyCode.W)) Spawn(DirectionType.Up);
        else if (Input.GetKeyDown(KeyCode.S)) Spawn(DirectionType.Down);
        else if (Input.GetKeyDown(KeyCode.A)) Spawn(DirectionType.Left);
        else if (Input.GetKeyDown(KeyCode.D)) Spawn(DirectionType.Right);
    }

    //Spawn energy projectile and record direction
    void Spawn(DirectionType dir) 
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
=======
        string input = GetInputAsString();
        if (!string.IsNullOrEmpty(input) && input != lastInput)
        {
            SpawnEnergy(input);
            lastInput = input;
        }

        if (string.IsNullOrEmpty(input))
        {
            lastInput = "";
        }
    }

    private string GetInputAsString()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) return "Up";
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) return "Down";
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) return "Left";
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return "Right";
        return "";
    }

    private void SpawnEnergy(string input)
    {
        if (shootCount >= maxShootCount) return;

        Vector3 spawnPos = playerTransform.position + (Vector3)spawnOffset;
        GameObject energy = Instantiate(energyPrefab, spawnPos, Quaternion.identity);

        if (energy.TryGetComponent(out EnergyProjectile projectile))
        {
            projectile.SetTarget(targetTransform);
            recordedMoves.Add(input);
            projectile.SetComboData(recordedMoves, shootCount);
        }

        shootCount++;

        if (shootCount >= maxShootCount)
        {
            canReadInput = false;
        }
>>>>>>> Stashed changes
    }
}
