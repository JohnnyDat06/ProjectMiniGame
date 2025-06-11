using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Animator playerAnimator;

    [Header("Movement Settings")]
    public float moveDistance = 5f; 
    public float moveDuration = 0.2f; 

    private bool isMoving = false;

    void Update()
    {
        Movement();
    }

    //Handles player movement based on input
    private void Movement()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(MovePlayer(Vector2.right * moveDistance));
                playerSpriteRenderer.flipX = false; // Face right
                playerAnimator.SetTrigger("IsMove");
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(MovePlayer(Vector2.left * moveDistance));
                playerSpriteRenderer.flipX = true; // Face left
                playerAnimator.SetTrigger("IsMove");
            }
        }
    }

    //Player movement coroutine
    IEnumerator MovePlayer(Vector2 direction)
    {
        isMoving = true;

        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + direction;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, (elapsedTime / moveDuration));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;

        isMoving = false; 
    }
}