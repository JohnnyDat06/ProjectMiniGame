using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Tilemap to Toggle")]
    [SerializeField] private GameObject tilemapToToggle;

    [Header("Animator for Plate Pressed Animation")]
    [SerializeField] private Animator animator;

    [Header("Sound when stepping on plate")]
    [SerializeField] private AudioClip pressSound;

    private int playersOnPlate = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnPlate++;
            animator.SetBool("Pressed", true);
            tilemapToToggle?.SetActive(false);

            // Phát âm thanh tại vị trí hiện tại
            if (pressSound != null)
                AudioSource.PlayClipAtPoint(pressSound, transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnPlate = Mathf.Max(0, playersOnPlate - 1);
            if (playersOnPlate == 0)
            {
                animator.SetBool("Pressed", false);
                tilemapToToggle?.SetActive(true);
            }
        }
    }
}
