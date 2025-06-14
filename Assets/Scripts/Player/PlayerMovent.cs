using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles 2D grid-based horizontal movement and ladder climbing.
/// </summary>
public class PlayerGridMovement : MonoBehaviour
{
	[Header("Movement Settings")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float moveDelay = 0.5f;
	[SerializeField] private float launchForceY = 8f;

	[Header("Physics Layers")]
	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private LayerMask terrainLayer;

	[Header("References")]
	[SerializeField] private Collider2D playerCollider;
	[SerializeField] private Rigidbody2D playerRigidbody;
	[SerializeField] private Transform playerTransform;
	[SerializeField] public Animator playerAnimator;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private Vector3 targetPosition;
	private bool isMoving = false;
	private bool canMove = true;
	private bool isOnStairBase = false;
	private bool isOnLadder = false;
	private GameObject currentPlatform = null;

	public List<string> moveHistory = new List<string>();
	public const int maxSteps = 7;

	private float downRecordCooldown = 1.2f;
	private float lastDownTime = -999f;

	void Start()
	{
		targetPosition = transform.position;
	}

	void Update()
	{
		if (!canMove || isMoving) return;

		if (canMove && IsGrounded())
		{
			Movement();
		}

		if (isOnStairBase && Input.GetAxisRaw("Vertical") > 0)
		{
			LaunchUp();
			RecordMove("up");
		}

		UpdateAnimation();
	}

	/// <summary>
	/// Handles left/right movement in grid steps
	/// </summary>
	private void Movement()
	{
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		if (horizontalInput != 0)
		{
			Vector3 direction = new Vector3(horizontalInput, 0f, 0f);

			if (CanMove(direction))
			{
				targetPosition = transform.position + direction;
				StartCoroutine(MoveToTargetPosition());

				RecordMove(horizontalInput < 0 ? "left" : "right");
			}
			else
			{
				Debug.Log("Blocked! Cannot move " + (horizontalInput < 0 ? "LEFT" : "RIGHT"));
			}
		}

		if (vertical < -0.5f && currentPlatform != null)
		{
			PlatformOnLadder platform = currentPlatform.GetComponent<PlatformOnLadder>();
			if (platform != null)
			{
				platform.DropPlayer();
				playerRigidbody.AddForce(Vector2.down * 0.05f, ForceMode2D.Impulse);

				if (Time.time - lastDownTime > downRecordCooldown)
				{
					RecordMove("down");
					lastDownTime = Time.time;
				}
			}
		}
	}

	private IEnumerator MoveToTargetPosition()
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

		UpdateAnimation();

		yield return new WaitForSeconds(moveDelay);
		canMove = true;

		PlayerStep(); // Notify the clone!

	}

	/// <summary>
	/// Check for obstacles before moving
	/// </summary>
	private bool CanMove(Vector3 direction)
	{
		float checkDistance = 1f;
		Vector3 origin = transform.position;

		RaycastHit2D hit = Physics2D.Raycast(origin, direction, checkDistance, obstacleLayer);

		Color rayColor = hit.collider ? Color.red : Color.green;
		Debug.DrawRay(origin, direction.normalized * checkDistance, rayColor, 0.1f);

		return hit.collider == null;
	}

	private bool IsGrounded()
	{
		return Physics2D.BoxCast(
			playerCollider.bounds.center,
			playerCollider.bounds.size,
			0f,
			Vector2.down,
			0.1f,
			terrainLayer
		);
	}

	private void LaunchUp()
	{
		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.AddForce(Vector2.up * launchForceY, ForceMode2D.Impulse);
		isOnStairBase = false;
	}

	private void UpdateAnimation()
	{
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float verticalInput = Input.GetAxisRaw("Vertical");

		if (horizontalInput < 0)
		{
			spriteRenderer.flipX = true;
		}
		else if (horizontalInput > 0)
		{
			spriteRenderer.flipX = false;
		}

		if (isOnLadder && Mathf.Abs(verticalInput) > 0)
		{
			playerAnimator.SetBool("IsMove", false);
			playerAnimator.SetTrigger("IsClimb");
		}

		playerAnimator.SetBool("IsMove", isMoving);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("StairBase"))
		{
			isOnStairBase = true;
		}

		if (collision.CompareTag("Ladder"))
		{
			isOnLadder = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("StairBase"))
		{
			isOnStairBase = false;
		}

		if (collision.CompareTag("Ladder"))
		{
			isOnLadder = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("PlatformOnLadder"))
		{
			ContactPoint2D[] contacts = new ContactPoint2D[1];
			collision.GetContacts(contacts);
			if (contacts[0].normal.y > 0.5f)
			{
				currentPlatform = collision.gameObject;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject == currentPlatform)
		{
			currentPlatform = null;
		}
	}

	/// <summary>
	/// Record a movement step (maxSteps limited)
	/// </summary>
	private void RecordMove(string move)
	{
		if (moveHistory.Count >= maxSteps) return;

		moveHistory.Add(move);
		Debug.Log("Step recorded: " + move);

		if (moveHistory.Count == maxSteps)
		{
			string history = string.Join(" → ", moveHistory);
			Debug.Log("7-step path complete: " + history);

			FindObjectOfType<GameManager>().SummonPlayerClone(moveHistory);
		}
	}

	public static event System.Action OnPlayerStep;

	private void PlayerStep()
	{
		OnPlayerStep?.Invoke(); // Notify clone to move
	}
}