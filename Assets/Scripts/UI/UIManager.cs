using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Các nút điều khiển mũi tên")]
    [SerializeField] private Button buttonUp;
    [SerializeField] private Button buttonDown;
    [SerializeField] private Button buttonLeft;
    [SerializeField] private Button buttonRight;

    private void Start()
    {
        // Gán sự kiện cho các nút
        if (buttonUp != null)
            buttonUp.onClick.AddListener(() => ArrowManager.Instance.OnArrowButtonPressed(ArrowDirection.Up));
        if (buttonDown != null)
            buttonDown.onClick.AddListener(() => ArrowManager.Instance.OnArrowButtonPressed(ArrowDirection.Down));
        if (buttonLeft != null)
            buttonLeft.onClick.AddListener(() => ArrowManager.Instance.OnArrowButtonPressed(ArrowDirection.Left));
        if (buttonRight != null)
            buttonRight.onClick.AddListener(() => ArrowManager.Instance.OnArrowButtonPressed(ArrowDirection.Right));
    }

    private void Update()
    {
        ArrowUIHorizontal();
        ArrowUIVertical();
    }

    private static void ArrowUIHorizontal()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ArrowManager.Instance.OnArrowButtonPressed(ArrowDirection.Left);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ArrowManager.Instance.OnArrowButtonPressed(ArrowDirection.Right);
        }
    }



    private static void ArrowUIVertical()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ArrowManager.Instance.OnArrowButtonPressed(ArrowDirection.Up);
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ArrowManager.Instance.OnArrowButtonPressed(ArrowDirection.Down);
        }
    }
}