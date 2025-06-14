using UnityEngine;
using UnityEngine.SceneManagement; // Cần thiết để sử dụng SceneManager
using System.Collections; // Cần thiết để sử dụng Coroutine

public class FlagManager : MonoBehaviour
{
    // Cài đặt cờ
    [SerializeField] Animator animatorFlag; // Gán Animator của cờ vào đây trong Inspector

    // Thời gian chờ trước khi chuyển scene
    [SerializeField] private float sceneChangeDelay = 3f;

    private bool hasTriggered = false;

    // Được gọi khi một Collider 2D khác chạm vào trigger của cờ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đã kích hoạt chưa và có phải là "Player" không
        if (!hasTriggered && collision.CompareTag("Player"))
        {
            hasTriggered = true; // Đánh dấu đã kích hoạt để không kích hoạt lại

            // Nếu có Animator, bật animation của cờ
            if (animatorFlag != null)
            {
                animatorFlag.SetInteger("State", 1);
            }

            // Bắt đầu Coroutine để chờ và chuyển scene
            StartCoroutine(DelayAndLoadNextScene());
        }
    }

    // Coroutine chờ và tải scene tiếp theo
    private IEnumerator DelayAndLoadNextScene()
    {
        // Chờ một khoảng thời gian
        yield return new WaitForSeconds(sceneChangeDelay);

        // Lấy build index của scene hiện tại
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Tính toán build index của scene tiếp theo (luôn là +1)
        int nextSceneIndex = currentSceneIndex + 1;

        // Kiểm tra xem scene tiếp theo có tồn tại không
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Tải scene tiếp theo theo build index
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Nếu không có scene tiếp theo (đã là scene cuối cùng)
            Debug.LogWarning("Không có scene tiếp theo trong Build Settings. Đã đạt đến scene cuối cùng.");
            // Tùy chọn: Bạn có thể tải lại scene hiện tại hoặc chuyển đến một scene menu chính
            // SceneManager.LoadScene(currentSceneIndex);
            // Hoặc Application.Quit(); để thoát game nếu đây là cảnh cuối cùng.
        }
    }
}