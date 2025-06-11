using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private float inputX;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update() => inputX = Input.GetAxisRaw("Horizontal");

    void FixedUpdate()
    {
        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Va chạm với: " + collision.gameObject.name);
    }

}
