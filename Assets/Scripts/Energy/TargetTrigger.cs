using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    //Handle projectile hit and trigger clone if it's the 7th (index == 6)
    public void RegisterHit(int index, List<string> comboData)
    {
        if (index == 6) // viên đạn thứ 7 có index = 6
        {
            if (gameManager != null)
            {
                gameManager.UnlockClone();
                gameManager.SummonPlayerClone(comboData);
                gameObject.SetActive(false); // Ẩn Target sau khi hoàn tất
            }
            else
            {
                Debug.LogWarning("⚠️ GameManager chưa được gán!");
            }
        }
    }
}
