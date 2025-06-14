using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public void RegisterHit(int comboIndex, List<string> comboData)
    {
        if (comboIndex == 6) // Chỉ viên đạn thứ 7 mới gọi clone (bắt đầu từ 0)
        {
            gameManager.SummonPlayerClone(comboData);
        }
    }
}
