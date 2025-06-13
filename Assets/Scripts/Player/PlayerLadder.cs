using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadder : MonoBehaviour
{
	[Header("Stair Jump Settings")]
	[SerializeField] private float launchForceY = 8f;

	[SerializeField] private Rigidbody2D playerRigidbody;
	[SerializeField] public Animator playerAnimator;

	private bool isOnStairBase = false;
	private bool isOnLadder = false;

	void Update()
	{
		// Xử lý bay lên khi đang ở stair base và nhấn lên
		if (isOnStairBase && Input.GetAxisRaw("Vertical") > 0)
		{
			LaunchUp();
		}

		float verticalInput = Input.GetAxisRaw("Vertical");

		if (isOnLadder && Mathf.Abs(verticalInput) > 0)
		{
			playerAnimator.SetBool("IsClimb", true);
		}
		else
		{
			playerAnimator.SetBool("IsClimb", false);
		}
	}

	private void LaunchUp()
	{
		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.AddForce(Vector2.up * launchForceY, ForceMode2D.Impulse);
		Debug.Log("leo len thang");
		isOnStairBase = false; // tránh spam nhảy
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("StairBase"))
		{
			isOnStairBase = true;
			Debug.Log("Chạm chân cầu thang");
		}

		if (collision.CompareTag("Ladder"))
		{
			isOnLadder = true;
			Debug.Log("Đang trên thang");

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
			Debug.Log("Thoát khỏi thang");
		}
	}

}
