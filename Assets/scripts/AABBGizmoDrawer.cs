using UnityEngine;

public class AABBGizmoDrawer : MonoBehaviour
{
    public Vector3 min;
    public Vector3 max;
    public Color gizmoColor = Color.green;

    void OnDrawGizmos()
{
    Gizmos.color = gizmoColor;

    Vector3 scale = transform.localScale;
    Vector3 position = transform.position;

    Vector3 halfSize = scale / 2f;
    Vector3 min = position - halfSize;
    Vector3 max = position + halfSize;

    Vector3 center = position;
    Vector3 size = max - min;

    Gizmos.DrawWireCube(center, size);
}

}
