using UnityEngine;

public class PressurePlateShow : MonoBehaviour
{
    [Header("Tilemap to Show (nên để sẵn là inactive)")]
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

            // Hiện tilemap
            tilemapToToggle?.SetActive(true);

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

                // Ẩn tilemap nếu không còn ai đứng trên plate
                tilemapToToggle?.SetActive(false);
            }
        }
    }
}
