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

    // Phương thức để gọi từ các nút UI
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

    // Giai đoạn 1: Sinh mũi tên
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

    // Chuẩn bị cho Giai đoạn 2
    private void StartPhase2()
    {
        if (spawnedArrows.Count > 0 && currentPositionMarker != null)
        {
            // Di chuyển marker đến vị trí đầu tiên để đánh dấu mũi tên sẽ bị xóa
            UpdatePositionMarker(0);
            currentPositionMarker.SetActive(true);
        }
        else if (spawnedArrows.Count == 0)
        {           
            currentStage = 3;
            StartPhase3();
        }
    }

    // Giai đoạn 2: Xóa mũi tên và di chuyển marker
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

    // Giai đoạn 3: Tắt tất cả UI
    private void StartPhase3()
    {
        if (currentPositionMarker != null)
        {
            currentPositionMarker.SetActive(false); // Đảm bảo marker ẩn
        }

        // Đảm bảo tất cả mũi tên còn lại (nếu có lỗi gì đó) cũng được hủy
        foreach (var arrowData in spawnedArrows.ToList()) // Sử dụng ToList() để tránh lỗi khi sửa đổi danh sách
        {
            if (arrowData.ArrowGameObject != null)
            {
                Destroy(arrowData.ArrowGameObject);
            }
            spawnedArrows.Remove(arrowData);
        }
        spawnedArrows.Clear(); // Đảm bảo danh sách rỗng

        foreach (GameObject uiElement in uiElementsToHide)
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false);
            }
        }   
    }
}