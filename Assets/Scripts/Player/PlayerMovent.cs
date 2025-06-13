using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 🇬🇧 Handles 2D grid-based horizontal movement and ladder climbing.
/// 🇻🇳 Xử lý di chuyển ngang dạng lưới và leo thang cho nhân vật 2D.
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

	// 🇬🇧 Initialize target position
	// 🇻🇳 Khởi tạo vị trí đích ban đầu
	void Start()
    {
        targetPosition = transform.position;
    }

    // 🇬🇧 Update input and state every frame
    // 🇻🇳 Cập nhật input và trạng thái mỗi khung hình
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
        }

        UpdateAnimation(); // Always update animation every frame
    }

    // 🇬🇧 Handles left/right movement in grid steps
    // 🇻🇳 Xử lý di chuyển trái/phải theo từng bước dạng lưới
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
            }
            else
            {
                Debug.Log("Bị chặn! Không đi được hướng " + (horizontalInput < 0 ? "TRÁI" : "PHẢI"));
            }
        }

		//xuong thang
		if (vertical < -0.5f)
		{
			if (currentPlatform != null)
			{
				PlatformOnLadder platform = currentPlatform.GetComponent<PlatformOnLadder>();
				if (platform != null)
				{
					platform.DropPlayer();
					playerRigidbody.AddForce(Vector2.down * 0.05f, ForceMode2D.Impulse);
				}
			}
		}
	}

    // 🇬🇧 Check for obstacles before moving
    // 🇻🇳 Kiểm tra vật cản trước khi di chuyển
    private bool CanMove(Vector3 direction)
    {
        float checkDistance = 1f;
        Vector3 origin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, checkDistance, obstacleLayer);

        // Debug ray in Scene view
        Color rayColor = hit.collider ? Color.red : Color.green;
        Debug.DrawRay(origin, direction.normalized * checkDistance, rayColor, 0.1f);

        return hit.collider == null;
    }

    // 🇬🇧 Smoothly move player to the next grid cell
    // 🇻🇳 Di chuyển nhân vật đến ô lưới kế tiếp một cách mượt
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

		UpdateAnimation(); //Stop animation when not moving
        // Wait for a delay before allowing next movement
        Debug.Log("Đã di chuyển đến vị trí: " + targetPosition);

		yield return new WaitForSeconds(moveDelay);
        canMove = true;
    }

    // 🇬🇧 Update sprite animation based on input and ladder state
    // 🇻🇳 Cập nhật animation dựa trên input và trạng thái thang
    private void UpdateAnimation()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Flip sprite depending on direction
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Climbing animation
        if (isOnLadder && Mathf.Abs(verticalInput) > 0)
        {
            playerAnimator.SetBool("IsMove", false); // Disable walk animation while climbing
			playerAnimator.SetTrigger("IsClimb");
        }
            

		// FIX: chỉ bật IsMove nếu thật sự đang trong coroutine di chuyển
		if (isMoving)
		{
			playerAnimator.SetBool("IsMove", true);
		}
		else
		{
			playerAnimator.SetBool("IsMove", false);
		}
	}

    // 🇬🇧 Check if the player is standing on ground (for movement)
    // 🇻🇳 Kiểm tra xem nhân vật có đang đứng trên mặt đất hay không
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

    // 🇬🇧 Launch player upward when standing at the base of stairs
    // 🇻🇳 Nhảy lên khi đang đứng ở chân cầu thang
    private void LaunchUp()
    {
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.AddForce(Vector2.up * launchForceY, ForceMode2D.Impulse);
        Debug.Log("leo lên thang");
        isOnStairBase = false; // Prevent multiple jumps
    }

    // 🇬🇧 Detect triggers (stair base and ladder)
    // 🇻🇳 Nhận diện va chạm với chân cầu thang và thang
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

    // 🇬🇧 Reset state when exiting trigger zones
    // 🇻🇳 Xoá trạng thái khi rời khỏi vùng trigger
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

	// 🇬🇧 Detect when player lands on a platform with a ladder
	// 🇻🇳 Nhận diện khi nhân vật rơi xuống một platform trên thang
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
