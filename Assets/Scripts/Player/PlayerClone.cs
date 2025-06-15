using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clone logic: plays back recorded 2D grid movement with animation and stair climbing
/// </summary>
public class PlayerClone : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float moveDelay = 0.1f;
	[SerializeField] private float launchForceY = 8f;

	[Header("Physics Layers")]
	[SerializeField] private LayerMask obstacleLayer;

	[Header("References")]
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private Animator animator;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private Collider2D playerCollider;

	private List<string> replayMoves = new List<string>();
	private int currentMoveIndex = 0;
	private bool isReplaying = false;
	private bool isMoving = false;
	private bool isOnStairBase = false;
	private bool isOnLadder = false;

	private Vector3 targetPosition;
	private GameObject currentPlatform = null; // Platform reference for drop logic
	private bool waitForFirstStep = true; // Wait until player makes next valid move


	private void OnEnable()
	{
		PlayerGridMovement.OnPlayerStep += OnPlayerStep;
	}

	private void OnDisable()
	{
		PlayerGridMovement.OnPlayerStep -= OnPlayerStep;
	}

	public void SetReplayPath(List<string> moves)
	{
		replayMoves = new List<string>(moves);
		targetPosition = transform.position;
		currentMoveIndex = 0; // Start from first step
		waitForFirstStep = true; // Delay replay until next step

	}

	private void OnPlayerStep()
	{
		// Wait for first step before starting replay
		if (waitForFirstStep)
		{
			waitForFirstStep = false;
			return;
		}

		if (!isReplaying && currentMoveIndex < replayMoves.Count)
		{
			StartCoroutine(ReplayStep());
		}
	}


	private IEnumerator ReplayStep()
	{
		isReplaying = true;

		string move = replayMoves[currentMoveIndex];
		Vector3 direction = Vector3.zero;

		if (move == "left")
		{
			direction = Vector3.left;
			spriteRenderer.flipX = true;
		}
		else if (move == "right")
		{
			direction = Vector3.right;
			spriteRenderer.flipX = false;
		}
		else if (move == "up")
		{
			// Check stair base before launching up
			if (isOnStairBase)
			{
				rb.velocity = Vector2.zero;
				rb.AddForce(Vector2.up * launchForceY, ForceMode2D.Impulse);
				animator.SetTrigger("IsClimb");
				isOnStairBase = false;
			}

			currentMoveIndex++;
			yield return new WaitForSeconds(moveDelay);
			isReplaying = false;
			yield break;
		}
		else if (move == "down")
		{
			// Drop through platform like player
			if (currentPlatform != null)
			{
				PlatformOnLadder platform = currentPlatform.GetComponent<PlatformOnLadder>();
				if (platform != null)
				{
					platform.DropPlayer(); // Disable collision to drop
				}
			}

			rb.AddForce(Vector2.down * 0.05f, ForceMode2D.Impulse);
			currentMoveIndex++;
			yield return new WaitForSeconds(moveDelay);
			isReplaying = false;
			yield break;
		}

		// Horizontal movement
		animator.SetBool("IsMove", true);
		targetPosition = transform.position + direction;
		isMoving = true;

		while ((transform.position - targetPosition).sqrMagnitude > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}

		transform.position = targetPosition;
		isMoving = false;
		animator.SetBool("IsMove", false);

		currentMoveIndex++;
		yield return new WaitForSeconds(moveDelay);
		isReplaying = false;
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
}
