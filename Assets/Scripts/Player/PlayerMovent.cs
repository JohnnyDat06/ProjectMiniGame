using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player movement in 2D grid (left/right 1 unit per input)
/// </summary>
public class PlayerGridMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveDelay = 0.6f; // Delay giữa các bước

    [SerializeField] private LayerMask obstacleLayer;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool canMove = true;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!canMove || isMoving) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            Vector3 direction = new Vector3(horizontalInput, 0f, 0f);

            if (CanMove(direction))
            {
                targetPosition = transform.position + direction;
                StartCoroutine(MoveToTargetPosition());
            }
            else
            {
                Debug.Log("🚧 Bị chặn! Không đi được hướng " + (horizontalInput < 0 ? "TRÁI" : "PHẢI"));
            }
        }
    }

    bool CanMove(Vector3 direction)
    {
        float checkDistance = 1f;
        Vector3 origin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, checkDistance, obstacleLayer);

        // Debug ray
        Color rayColor = hit.collider ? Color.red : Color.green;
        Debug.DrawRay(origin, direction.normalized * checkDistance, rayColor, 0.1f);

        return hit.collider == null;
    }

    System.Collections.IEnumerator MoveToTargetPosition()
    {
        isMoving = true;
        canMove = false;

        while ((transform.position - targetPosition).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;

        // ⏳ Delay sau khi di chuyển xong
        yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }
}
