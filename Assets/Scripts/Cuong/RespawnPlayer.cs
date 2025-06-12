using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPlayer : MonoBehaviour
{
    //Moves the player to the last checkpoint position when the scene starts,if a checkpoint has been set
    private void Start()
    {
        if (CheckpointManager.HasCheckpoint)
        {
            transform.position = CheckpointManager.LastCheckpointPosition;
        }
    }

    //Reloads the current scene
    public void Die()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    //Triggers the player's death if they touch a kill zone or enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("KillZone") || collision.CompareTag("Enemy"))
        {
            Die();
        }
    }
}
