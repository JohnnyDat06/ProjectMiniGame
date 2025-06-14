using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using TMPro;

public class ArrowManager : MonoBehaviour
{
    [Header("Instantiate arrow")]
    [SerializeField] private RectTransform[] arrowSpawnPositions = new RectTransform[7];
    [SerializeField] private GameObject arrowPrefabUp;
    [SerializeField] private GameObject arrowPrefabDown;
    [SerializeField] private GameObject arrowPrefabLeft;
    [SerializeField] private GameObject arrowPrefabRight;
    [SerializeField] private TextMeshProUGUI arrowCountText;
    [SerializeField] private int maxArrows = 7;
    private int currArrowsToSpawn = 7;

    [Header("Marker")]
    [SerializeField] private GameObject positionMarkerPrefab;
    [SerializeField] private float markerYOffset = -50f;
    private GameObject currentPositionMarker;
    private int currentMarkerDisplayIndex = 0;

    [Header("Off UI")]
    [SerializeField] private GameObject[] uiElementsToHide;

    private List<ArrowData> spawnedArrows = new List<ArrowData>();
    private int currentStage = 1; 

    public static ArrowManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currArrowsToSpawn = maxArrows;
        UpdateArrowCountText();
        if (positionMarkerPrefab != null)
        {
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                currentPositionMarker = Instantiate(positionMarkerPrefab, parentCanvas.transform);
                currentPositionMarker.SetActive(false);
            }
        }
    }
    private void UpdateArrowCountText()
    {
        if (arrowCountText != null)
        {
            arrowCountText.text = currArrowsToSpawn.ToString();
        }
    }

    // call this method when an arrow button is pressed
    public void OnArrowButtonPressed(ArrowDirection direction)
    {
        if (currentStage == 1)
        {
            SpawnArrow(direction);
        }
        else if (currentStage == 2)
        {
            RemoveArrowAndMoveMarker();
        }
    }

    // Spawn arrow based on the direction
    private void SpawnArrow(ArrowDirection direction)
    {
        if (spawnedArrows.Count < maxArrows)
        {
            RectTransform targetPosition = arrowSpawnPositions[spawnedArrows.Count];
            if (targetPosition == null)
            {
                return;
            }

            GameObject arrowPrefab = GetArrowPrefab(direction);
            if (arrowPrefab != null)
            {
                GameObject newArrowGO = Instantiate(arrowPrefab, targetPosition);

                RectTransform newArrowRect = newArrowGO.GetComponent<RectTransform>();
                if (newArrowRect != null)
                {
                    newArrowRect.anchoredPosition = Vector2.zero;
                }               
                spawnedArrows.Add(new ArrowData(direction, newArrowGO));

                currArrowsToSpawn--;
                UpdateArrowCountText();
                if (spawnedArrows.Count == maxArrows)
                {
                    currentStage = 2;
                    StartPhase2();
                }
            }
        }
    }
    // Get the appropriate arrow prefab based on the direction
    private GameObject GetArrowPrefab(ArrowDirection direction)
    {
        switch (direction)
        {
            case ArrowDirection.Up: return arrowPrefabUp;
            case ArrowDirection.Down: return arrowPrefabDown;
            case ArrowDirection.Left: return arrowPrefabLeft;
            case ArrowDirection.Right: return arrowPrefabRight;
            default: return null;
        }
    }

    // prepare for phase 2 when all arrows are spawned
    private void StartPhase2()
    {
        if (spawnedArrows.Count > 0 && currentPositionMarker != null)
        {
            UpdatePositionMarker(0);
            currentPositionMarker.SetActive(true);
        }
        else if (spawnedArrows.Count == 0)
        {           
            currentStage = 3;
            StartPhase3();
        }
    }

    // Remove the arrow and move the marker to the next position
    private void RemoveArrowAndMoveMarker()
    {
        if(currentStage == 2)
        {
            if (spawnedArrows.Count > 0)
            {
                GameObject positionToHide = arrowSpawnPositions[currentMarkerDisplayIndex].gameObject;
                if (positionToHide != null)
                {
                    positionToHide.SetActive(false);
                }

                Destroy(spawnedArrows[0].ArrowGameObject);
                spawnedArrows.RemoveAt(0);
            }

            currentMarkerDisplayIndex++;

            if (currentMarkerDisplayIndex < maxArrows)
            {
                UpdatePositionMarker(currentMarkerDisplayIndex);
            }
            else
            {
                currentStage = 3;
                StartPhase3();
            }
        }      
    }

    private void UpdatePositionMarker(int targetListIndex)
    {
        if (currentPositionMarker == null || targetListIndex < 0 || targetListIndex >= arrowSpawnPositions.Length)
        {
            currentPositionMarker.SetActive(false);
            return;
        }
        Vector3 targetPosition = arrowSpawnPositions[targetListIndex].position; 
        targetPosition.y += markerYOffset;
        currentPositionMarker.transform.position = targetPosition;
        currentPositionMarker.SetActive(true);
    }

    // turn off all UI elements and clear arrows
    private void StartPhase3()
    {
        if (currentPositionMarker != null)
        {
            currentPositionMarker.SetActive(false);
        }

        foreach (var arrowData in spawnedArrows.ToList())
        {
            if (arrowData.ArrowGameObject != null)
            {
                Destroy(arrowData.ArrowGameObject);
            }
            spawnedArrows.Remove(arrowData);
        }
        spawnedArrows.Clear();

        foreach (GameObject uiElement in uiElementsToHide)
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false);
            }
        }   
    }
}