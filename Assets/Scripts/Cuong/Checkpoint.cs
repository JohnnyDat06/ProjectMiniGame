using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //When the player enters the checkpoint's trigger collider
    //this method saves the checkpoint position using the CheckpointManager
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckpointManager.SetCheckpoint(transform.position);
        }
    }
}
