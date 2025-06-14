using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private bool hasTriggered = false;

    public void RegisterHit(List<string> comboData)
    {
        if (hasTriggered) return;

        if (comboData.Count == 7 && gameManager != null)
        {
            gameManager.TrySummonFromTarget(comboData);
            hasTriggered = true;
            gameObject.SetActive(false);
        }
    }
}
