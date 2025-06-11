using UnityEngine;

public class PressurePlateManager : MonoBehaviour
{
    [Header("Put the tilemap you want to disappear here")]
    [SerializeField] private GameObject tilemapToToggle;
    [Header("Drag the Animator with Pressed animation here")]
    [SerializeField] private Animator animator;
    private int playersOnPlate = 0;
    //The "Player" tag is touched and the Animation set to SetBool is triggered causing the PressurePlate to press down and the "tilemapToToggle" will temporarily hide if the "Player" is still standing there
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnPlate++;
            animator.SetBool("Pressed", true);
            if (tilemapToToggle != null)
                tilemapToToggle.SetActive(false);
        }
    }
    //The "Player" tag leaves the collision then the set Animation will return to the beginning and tilemapToToggle will be enabled
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnPlate = Mathf.Max(0, playersOnPlate - 1);
            if (playersOnPlate == 0)
            {
                animator.SetBool("Pressed", false);
                if (tilemapToToggle != null)
                    tilemapToToggle.SetActive(true);
            }
        }
    }
}
