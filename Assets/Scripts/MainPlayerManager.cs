using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MainPlayerManager : MonoBehaviour
{
	public float movementSpeed = 5f;
	public float climbSpeed = 3f;
	public LayerMask obstacleLayer;

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
	/// Handles ladder movement: smooth climbing up/down when inside ladder trigger.
	/// </summary>
	void HandleLadderMovement()
	{
		float verticalInput = Input.GetAxisRaw("Vertical");

		Vector3 direction = new Vector3(0, verticalInput, 0);

		if (CanClimb(direction))
		{
			transform.position += direction * climbSpeed * Time.deltaTime;
		}
	}

	/// <summary>
	/// Checks whether the player can move to the next tile (no obstacle).
	/// </summary>
	bool CanMove(Vector3 direction)
	{
		float distance = 1f;
		Vector3 origin = transform.position;

		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, obstacleLayer);

		// 💡 Vẽ ray để debug trong Scene view
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
