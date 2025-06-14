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
    [Header("Prefab and related objects")]
    [SerializeField] private GameObject energyPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector2 spawnOffset = Vector2.up;
    [SerializeField] private PlayerGridMovement playerGridMovement;

    [Header("UI Manager")]
    [SerializeField] private UIManager uiManager;

    [Header("Shoot Settings")]
    [SerializeField] private int maxShootCount = 7;

    private bool canReadInput = true;
    private DirectionType lastInput = DirectionType.None;
    private int shootCount = 0;

    private void Update()
    {
        if (!canReadInput) return;

        DirectionType input = GetInputFromKey();
        if (input != DirectionType.None && input != lastInput)
        {
            SpawnEnergy();
            lastInput = input;
        }

        if (input == DirectionType.None)
        {
            lastInput = DirectionType.None;
        }
    }

    private DirectionType GetInputFromKey()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) return DirectionType.Up;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) return DirectionType.Down;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) return DirectionType.Left;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) return DirectionType.Right;
        return DirectionType.None;
    }

    private void SpawnEnergy()
    {
        if (shootCount >= maxShootCount)
        {
            canReadInput = false;
            return;
        }

        if (energyPrefab != null && playerTransform != null)
        {
            Vector3 spawnPos = playerTransform.position + (Vector3)spawnOffset;
            GameObject energy = Instantiate(energyPrefab, spawnPos, Quaternion.identity);

            if (energy.TryGetComponent(out EnergyProjectile projectile) && targetTransform != null)
            {
                projectile.SetTarget(targetTransform);
            }

            shootCount++;

            if (shootCount >= maxShootCount)
            {
                canReadInput = false;
            }
        }
    }
}