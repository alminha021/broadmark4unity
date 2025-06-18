using UnityEngine;

public class AABBObjectController : MonoBehaviour
{
    public Vector3 velocity;
    public float speed = 5f;
    public Vector3 min;
    public Vector3 max;

    private float halfSize = 1f;
    private Renderer rend;

    void Start()
    {
        velocity = Random.onUnitSphere * speed;
        halfSize = transform.localScale.x / 2f;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;

        Vector3 pos = transform.position;
        for (int i = 0; i < 3; i++)
        {
            if (pos[i] > 50f) { pos[i] = 50f; velocity[i] *= -1; }
            if (pos[i] < -50f) { pos[i] = -50f; velocity[i] *= -1; }
        }
        transform.position = pos;

        UpdateAABB();
    }

    public void UpdateAABB()
    {
        Vector3 p = transform.position;
        min = p - Vector3.one * halfSize;
        max = p + Vector3.one * halfSize;
    }

    public void SetColor(Color c)
    {
        if (rend != null)
            rend.material.color = c;
    }
}
