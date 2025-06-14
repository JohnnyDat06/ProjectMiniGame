using UnityEngine;
using System.Collections;

// Enum for movement directions used in combo input
public enum DirectionType
{
    None,
    Left,
    Right,
    Up,
    Down
}

public class ComboMovementEnergySpawner : MonoBehaviour
{
    [Header("Energy projectile prefab")]
    [SerializeField] private GameObject energyPrefab; 

    [Header("Reference to the player transform")]
    [SerializeField] private Transform playerTransform; 

    [Header("Target the projectile will move toward")]
    [SerializeField] private Transform targetTransform; 

    [Header("Spawn offset relative to the player")]
    [SerializeField] private Vector2 spawnOffset = Vector2.up; 
    [Header("Player's initial position (used to reset on combo fail)")]
    [SerializeField] private Transform startingPosition; 

    [Header("Combo sequence of movement directions")]
    [SerializeField]
    private DirectionType[] comboSequence = new DirectionType[]
    {
        DirectionType.Left,
        DirectionType.Right,
        DirectionType.Right,
        DirectionType.Up,
        DirectionType.Down
    }; 

    [Header("Reference to PlayerGridMovement")]
    [SerializeField] private PlayerGridMovement playerGridMovement; 

    private int currentComboIndex = 0; 
    private DirectionType lastDirection = DirectionType.None; 
    private bool canReadInput = true; 

    void Update()
    {
        if (!canReadInput) return;

        
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        DirectionType currentDirection = GetDirectionFromInput(input);

        
        if (currentDirection != DirectionType.None && currentDirection != lastDirection)
        {
            HandleComboStep(currentDirection);
            lastDirection = currentDirection;
        }

        
        if (currentDirection == DirectionType.None)
        {
            lastDirection = DirectionType.None;
        }
    }

    
    private DirectionType GetDirectionFromInput(Vector2 input)
    {
        if (input.x < 0) return DirectionType.Left;
        if (input.x > 0) return DirectionType.Right;
        if (input.y > 0) return DirectionType.Up;
        if (input.y < 0) return DirectionType.Down;
        return DirectionType.None;
    }

    // Check if input matches combo step
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
            StartCoroutine(ResetPlayerAndCombo()); 
        }
    }

    // Instantiate the energy projectile and set its target
    private void SpawnEnergy()
    {
        if (energyPrefab != null && playerTransform != null)
        {
            Vector3 spawnPos = playerTransform.position + (Vector3)spawnOffset;
            GameObject energy = Instantiate(energyPrefab, spawnPos, Quaternion.identity);
            EnergyProjectile projectile = energy.GetComponent<EnergyProjectile>();
            if (projectile != null && targetTransform != null)
            {
                projectile.SetTarget(targetTransform);
            }
        }
    }

    //Reset combo, player position, and movement
    private IEnumerator ResetPlayerAndCombo()
    {
        canReadInput = false;

        if (playerGridMovement != null)
        {
            playerGridMovement.enabled = false; 
            playerGridMovement.playerAnimator.SetBool("IsMove", false); 
        }

        if (startingPosition != null && playerTransform != null)
        {
            playerTransform.position = startingPosition.position; 

            Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
        currentComboIndex = 0;
        lastDirection = DirectionType.None;
        while (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        if (playerGridMovement != null)
        {
            playerGridMovement.enabled = true; 
        }
        canReadInput = true;
    }
}
