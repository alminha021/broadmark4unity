using UnityEngine;

public class BrownianMotion : MonoBehaviour
{
    [HideInInspector] public Vector3 boundsSize;
    [HideInInspector] public float speed;
    [HideInInspector] public float directionChangeInterval;

    private Vector3 direction;
    private float timeSinceLastChange = 0;

    void Start()
    {
        ChangeDirection();
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        timeSinceLastChange += Time.deltaTime;

        if (timeSinceLastChange >= directionChangeInterval)
        {
            ChangeDirection();
            timeSinceLastChange = 0;
        }

        ClampToBounds();
    }

    void ChangeDirection()
    {
        direction = Random.onUnitSphere.normalized;
    }

    void ClampToBounds()
    {
        Vector3 pos = transform.position;
        Vector3 halfBounds = boundsSize / 2f;

        if (Mathf.Abs(pos.x) > halfBounds.x) { direction.x *= -1; pos.x = Mathf.Sign(pos.x) * halfBounds.x; }
        if (Mathf.Abs(pos.y) > halfBounds.y) { direction.y *= -1; pos.y = Mathf.Sign(pos.y) * halfBounds.y; }
        if (Mathf.Abs(pos.z) > halfBounds.z) { direction.z *= -1; pos.z = Mathf.Sign(pos.z) * halfBounds.z; }

        transform.position = pos;
    }
}
