using UnityEngine;

public class AABBObjectController : MonoBehaviour
{
    public int id;  // ID fixo, atribuído externamente

    public Vector3 min;
    public Vector3 max;

    private float halfSize = 1f;
    private Renderer rend;

    void Start()
    {
        halfSize = transform.localScale.x / 2f;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
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
