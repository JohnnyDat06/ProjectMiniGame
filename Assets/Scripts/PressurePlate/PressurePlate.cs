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
        if (other == null || other.gameObject == null) return;

        if (other.CompareTag("Player"))
        {
            playersOnPlate++;
            if (animator != null) animator.SetBool("Pressed", true);
            if (tilemapToToggle != null) tilemapToToggle?.SetActive(false);

            if (pressSound != null) AudioSource.PlayClipAtPoint(pressSound, transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == null || other.gameObject == null) return;

        if (other.CompareTag("Player"))
        {
            playersOnPlate = Mathf.Max(0, playersOnPlate - 1);
            if (playersOnPlate == 0)
            {
                if(animator != null) animator.SetBool("Pressed", false);
                if (tilemapToToggle != null) tilemapToToggle?.SetActive(true);
            }
        }
    }
}
