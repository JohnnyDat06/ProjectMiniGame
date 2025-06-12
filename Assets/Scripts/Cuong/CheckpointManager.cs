using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    //Stores the last recorded checkpoint position
    public static Vector3 LastCheckpointPosition { get; private set; }
    //Indicates whether a checkpoint has been set
    public static bool HasCheckpoint { get; private set; } = false;

    //Initializes the first checkpoint at the player's current position if no checkpoint exists
    private void Awake()
    {
        
        if (!HasCheckpoint)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                LastCheckpointPosition = player.transform.position;
                HasCheckpoint = true;
            }
        }
    }

    //Saves the given position as the current checkpoint and marks that a checkpoint has been set
    public static void SetCheckpoint(Vector3 position)
    {
        LastCheckpointPosition = position;
        HasCheckpoint = true;
    }
}
