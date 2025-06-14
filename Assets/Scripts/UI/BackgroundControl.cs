using UnityEngine;

public class BackgroundScrollerSimple : MonoBehaviour
{
    public static BackgroundScrollerSimple Instance { get; private set; }

    [Header("Cài đặt cuộn background đơn giản")]
    [Tooltip("Tốc độ cuộn của background mỗi khi nhấn phím.")]
    [SerializeField] private float scrollSpeed = 0.5f;

    [Tooltip("Giới hạn cuộn tối đa của background về bên phải.")]
    [SerializeField] private float maxScrollRight = 10f; 

    [Tooltip("Giới hạn cuộn tối đa của background về bên trái.")]
    [SerializeField] private float maxScrollLeft = -10f; 

    private Vector3 initialBackgroundPosition;

    public float HorizontalInput { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        initialBackgroundPosition = transform.position;
    }

    void Update()
    {
        MoveBackground();
    }

    public void MoveBackground()
    {
        HorizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(HorizontalInput) > 0.01f)
        {
            float movementX = -HorizontalInput * scrollSpeed * Time.deltaTime;

            Vector3 newBackgroundPosition = transform.position;
            newBackgroundPosition.x += movementX;

            newBackgroundPosition.x = Mathf.Clamp(newBackgroundPosition.x, initialBackgroundPosition.x + maxScrollLeft, initialBackgroundPosition.x + maxScrollRight);

            transform.position = newBackgroundPosition;
        }
    }
}