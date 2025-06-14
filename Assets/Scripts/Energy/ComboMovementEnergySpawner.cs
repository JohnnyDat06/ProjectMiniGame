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
    [Header("Prefab và đối tượng liên quan")]
    [SerializeField] private GameObject energyPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector2 spawnOffset = Vector2.up;
    [SerializeField] private PlayerGridMovement playerGridMovement;

    [Header("UI Manager (không chỉnh sửa)")]
    [SerializeField] private UIManager uiManager;

    [Header("Combo cần thực hiện")]
    [SerializeField]
    private DirectionType[] comboSequence = new DirectionType[]
    {
        DirectionType.Left,
        DirectionType.Right,
        DirectionType.Right,
        DirectionType.Up,
        DirectionType.Down
    };

    private int currentComboIndex = 0;
    private bool canReadInput = true;
    private DirectionType lastInput = DirectionType.None;

    private void Update()
    {
        if (!canReadInput) return;

        DirectionType input = GetInputFromKey();
        if (input != DirectionType.None && input != lastInput)
        {
            HandleComboStep(input);
            lastInput = input;
        }

        if (input == DirectionType.None)
        {
            lastInput = DirectionType.None;
        }
    }

    private DirectionType GetInputFromKey()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.S)) return DirectionType.Up;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.W)) return DirectionType.Down;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) return DirectionType.Left;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) return DirectionType.Right;
        return DirectionType.None;
    }

    private void HandleComboStep(DirectionType inputDirection)
    {
        if (inputDirection == comboSequence[currentComboIndex])
        {
            SpawnEnergy();
            currentComboIndex++;

            if (currentComboIndex >= comboSequence.Length)
            {
                currentComboIndex = 0;
            }
        }
        else
        {
            StartCoroutine(ResetComboOnly());
        }
    }

    private void SpawnEnergy()
    {
        if (energyPrefab != null && playerTransform != null)
        {
            Vector3 spawnPos = playerTransform.position + (Vector3)spawnOffset;
            GameObject energy = Instantiate(energyPrefab, spawnPos, Quaternion.identity);

            if (energy.TryGetComponent(out EnergyProjectile projectile) && targetTransform != null)
            {
                projectile.SetTarget(targetTransform);
            }
        }
    }

    private System.Collections.IEnumerator ResetComboOnly()
    {
        canReadInput = false;
        currentComboIndex = 0;

        while (Input.anyKey)
            yield return null;

        yield return new WaitForSeconds(0.5f);
        canReadInput = true;
    }
}
