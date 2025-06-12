using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class UIManager : MonoBehaviour
{
    [Header("Arrow Prefabs")]
    [SerializeField] private GameObject upArrowPrefab;
    [SerializeField] private GameObject downArrowPrefab;
    [SerializeField] private GameObject leftArrowPrefab;
    [SerializeField] private GameObject rightArrowPrefab;

    [Header("Arrow Display Spots")]
    [SerializeField] private GameObject hideUI;
    [SerializeField] private GameObject[] arrowDisplaySpots;
    [SerializeField] private TextMeshProUGUI arrowCountText;

    private List<GameObject> activeArrowInstances = new List<GameObject>();
    private const int MAX_ARROWS = 7;

    void Start()
    {
        ResetArrows();
        if (hideUI != null)
        {
            hideUI.SetActive(true);
        }
    }

    // function to set up the arrow display spots
    public void ShowDirectionArrow(Direction direction)
    {
        if (activeArrowInstances.Count >= MAX_ARROWS)
        {
            hideUI.SetActive(false);
            ResetArrows();
        }
        // Get position of the next available arrow
        int currentArrowIndex = activeArrowInstances.Count;

        if (currentArrowIndex < MAX_ARROWS && arrowDisplaySpots[currentArrowIndex] != null)
        {
            GameObject arrowPrefabToInstantiate = GetArrowPrefab(direction);

            if (arrowPrefabToInstantiate != null)
            {
                GameObject newArrowInstance = Instantiate(
                    arrowPrefabToInstantiate,
                    arrowDisplaySpots[currentArrowIndex].transform.position,
                    arrowDisplaySpots[currentArrowIndex].transform.rotation,
                    arrowDisplaySpots[currentArrowIndex].transform
                );
                activeArrowInstances.Add(newArrowInstance);
                arrowCountText.text = (MAX_ARROWS - activeArrowInstances.Count).ToString();
            }
        }
    }

    // Destroy all active arrow instances and clear the list
    private void ResetArrows()
    {
        foreach (GameObject arrowInstance in activeArrowInstances)
        {
            if (arrowInstance != null)
            {
                Destroy(arrowInstance);
            }
        }
        activeArrowInstances.Clear();
    }

    // get prefab corresponding to the direction
    private GameObject GetArrowPrefab(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return upArrowPrefab;
            case Direction.Down: return downArrowPrefab;
            case Direction.Left: return leftArrowPrefab;
            case Direction.Right: return rightArrowPrefab;
            default: return null;
        }
    }
}