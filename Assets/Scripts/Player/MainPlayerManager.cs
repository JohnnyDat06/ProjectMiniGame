using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MainPlayerManager : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private float climbSpeed = 3f;

	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private LayerMask terrainLayer;

	[SerializeField] private Rigidbody2D playerRigidbody;
	[SerializeField] private Collider2D playerCollider;

	
	private bool isMoving = false;
	private Vector3 targetPosition;

	private enum MovementMode
	{
		GridMove,
		LadderMove
	}

	private MovementMode currentMovementMode = MovementMode.GridMove;

	void Start()
	{
		targetPosition = transform.position;
	}

	void Update()
	{
		if (!IsGrounded())
		{
			Fall();
			return; // không cho di chuyển nếu đang rơi
		}
		else
		{
			StickToGround();
		}

		switch (currentMovementMode)
		{
			case MovementMode.GridMove:
				HandleGridMovement();
				break;
			case MovementMode.LadderMove:
				HandleLadderMovement();
				break;
		}
	}

	/// <summary>
	/// Handles movement on a grid: moves one unit per input if no obstacles ahead.
	/// Xử lý chuyển động trên lưới: di chuyển một đơn vị cho mỗi lần nhập nếu không có chướng ngại vật phía trước.
	/// </summary>
	void HandleGridMovement()
	{
		if (isMoving) return;

		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float verticalInput = Input.GetAxisRaw("Vertical");

		// Only allow movement in one direction at a time
		if (horizontalInput != 0) verticalInput = 0;

		Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

		if (direction != Vector3.zero && CanMove(direction))
		{
			targetPosition = transform.position + direction;
			StartCoroutine(MoveToTargetPosition());
		}
	}

	/// <summary>
	/// Handles ladder movement: move 2 units per input press if there is a ladder above or below.
	/// </summary>
	void HandleLadderMovement()
	{
		if (isMoving) return;

		float verticalInput = Input.GetAxisRaw("Vertical");

		// Only proceed when there's an input
		if (verticalInput != 0)
		{
			Vector3 direction = new Vector3(0, verticalInput, 0);
			Vector3 climbTarget = transform.position + direction * 2f;

			// Use raycast to check if ladder exists 2 units ahead
			if (CanClimb(direction))
			{
				targetPosition = climbTarget;
				StartCoroutine(MoveToTargetPosition());
			}
		}
	}

	/// <summary>
	/// Checks if player is grounded using BoxCast.
	/// </summary>
	private bool IsGrounded()
	{
		return Physics2D.BoxCast(playerCollider.bounds.center,playerCollider.bounds.size,0f,
			Vector2.down,0.1f,terrainLayer
		);
	}

	/// <summary>
	/// Enables gravity to let the player fall.
	/// </summary>
	void Fall()
	{
		playerRigidbody.isKinematic = false;
		playerRigidbody.gravityScale = 1f;
	}

	/// <summary>
	/// Disables gravity and locks the player to the ground.
	/// </summary>
	void StickToGround()
	{
		playerRigidbody.isKinematic = true;
		playerRigidbody.gravityScale = 0f;
		playerRigidbody.velocity = Vector2.zero; // tránh dư đà
	}


	/// <summary>
	/// Checks whether the player can move to the next tile (no obstacle).
	/// </summary>
	bool CanMove(Vector3 direction)
	{
		float distance = 1f;
		Vector3 origin = transform.position;

		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, obstacleLayer);

		if (currentMovementMode == MovementMode.LadderMove)
			return true;

		// Vẽ ray để debug trong Scene view
		Color rayColor = hit.collider ? Color.red : Color.green;
		Debug.DrawRay(origin, direction.normalized * distance, rayColor);

		return hit.collider == null;
	}

	/// <summary>
	/// Checks whether there is a ladder in the climbing direction.
	/// </summary>
	bool CanClimb(Vector3 direction)
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.6f, obstacleLayer);
		return hit.collider != null && hit.collider.CompareTag("Ladder");
	}

	/// <summary>
	/// Smoothly moves the player to the target position.
	/// </summary>
	IEnumerator MoveToTargetPosition()
	{
		isMoving = true;
		while ((transform.position - targetPosition).sqrMagnitude > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
			yield return null;
		}
		transform.position = targetPosition;
		isMoving = false;
	}

	/// <summary>
	/// Detects when player enters a ladder trigger and enables ladder movement mode.
	/// </summary>
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Ladder"))
		{
			currentMovementMode = MovementMode.LadderMove;
		}
	}

	/// <summary>
	/// Detects when player exits a ladder trigger and returns to grid movement mode.
	/// </summary>
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Ladder"))
		{
			currentMovementMode = MovementMode.GridMove;
		}
	}
}
