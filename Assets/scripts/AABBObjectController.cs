using UnityEngine;

public class AABBObjectController : MonoBehaviour
{
    public int id;

    public Vector3 min;
    public Vector3 max;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        UpdateAABB();
    }

    public void UpdateAABB()
    {
        if (rend == null) return;

        Bounds b = rend.bounds;

        min = b.min;
        max = b.max;
    }

    public void SetColor(Color c)
    {
        if (rend != null)
            rend.material.color = c;
    }

    // 👇 Gizmo para visualizar o AABB em amarelo
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 size = max - min;
        Vector3 center = min + size * 0.5f;
        Gizmos.DrawWireCube(center, size);
    }
}
