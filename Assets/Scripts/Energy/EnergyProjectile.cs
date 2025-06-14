using UnityEngine;

public class EnergyProjectile : MonoBehaviour
{
    [Header("Projectile speed")]
    [SerializeField] private float speed = 10f;

    [Header("Target (assigned via script or Inspector)")]
    [SerializeField] private Transform targetTransform;

    private Vector3 targetPosition;
    private bool hasTarget = false;

    private void Start()
    {
        // Use assigned transform's current position if any
        if (targetTransform != null)
        {
            SetTarget(targetTransform.position);
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        hasTarget = true;
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    private void Update()
    {
        Vector3 targetPos = hasTarget ? targetPosition :
                           targetTransform != null ? targetTransform.position :
                           transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            Destroy(gameObject);
        }
    }
}
