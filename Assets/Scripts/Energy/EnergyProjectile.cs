using System.Collections.Generic;
using UnityEngine;

public class EnergyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Transform target;
    private List<string> comboData;
    private int comboIndex;

<<<<<<< Updated upstream
    //Assign the target transform for the projectile
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    //Set combo data and the index of this projectile
    public void SetComboData(List<string> data, int index)
    {
        comboData = new List<string>(data);
        comboIndex = index;
    }

    //Move the projectile towards the target and rotate it to face the direction
    private void Update()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Rotate to face direction
        Vector3 dir = target.position - transform.position;
        if (dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    //Check for collision with TargetTrigger and register the hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out TargetTrigger targetTrigger))
        {
            targetTrigger.RegisterHit(comboIndex, comboData);
=======
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void SetComboData(List<string> data, int index)
    {
        comboData = new(data);
        comboIndex = index;
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        transform.position += dir.normalized * speed * Time.deltaTime;

        if (dir != Vector3.zero)
            transform.right = dir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out TargetTrigger targetTrigger))
        {
            if (comboIndex == 6) // Viên thứ 7 (0-based index)
            {
                targetTrigger.RegisterHit(comboData);
            }

>>>>>>> Stashed changes
            Destroy(gameObject);
        }
    }
}
