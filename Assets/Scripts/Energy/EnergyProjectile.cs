using System.Collections.Generic;
using UnityEngine;

public class EnergyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Transform target;
    private List<string> comboData;
    private int comboIndex;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void SetComboData(List<string> data, int index)
    {
        comboData = new List<string>(data);
        comboIndex = index;
    }

    private void Update()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out TargetTrigger targetTrigger))
        {
            targetTrigger.RegisterHit(comboIndex, comboData);
            Destroy(gameObject);
        }
    }
}
