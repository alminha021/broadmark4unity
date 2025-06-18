using UnityEngine;

public class AABBGizmoDrawer : MonoBehaviour
{
    public Vector3 min;
    public Vector3 max;
    public Color gizmoColor = Color.green;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Vector3 center = (min + max) / 2f;
        Vector3 size = max - min;
        Gizmos.DrawWireCube(center, size);
    }
}
