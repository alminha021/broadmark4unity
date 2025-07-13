using UnityEngine;
using System.Collections.Generic;

public class WorldBoundsDrawer : MonoBehaviour
{
    [Header("World Bounds")]
    public Vector3 worldMin = new Vector3(-50, -50, -50);
    public Vector3 worldMax = new Vector3(50, 50, 50);
    public Color worldColor = Color.black;

    [Header("AABB Objects")]
    public Color aabbColor = Color.green;
    public List<GameObject> aabbObjects = new List<GameObject>();

    void OnDrawGizmos()
    {
        // Desenha o bounding box do mundo
        Gizmos.color = worldColor;
        Vector3 worldCenter = (worldMin + worldMax) / 2f;
        Vector3 worldSize = worldMax - worldMin;
        Gizmos.DrawWireCube(worldCenter, worldSize);

        // Desenha os AABBs dos objetos
        Gizmos.color = aabbColor;
        foreach (GameObject obj in aabbObjects)
        {
            if (obj == null) continue;

            Vector3 min = obj.transform.position - obj.transform.localScale / 2f;
            Vector3 max = obj.transform.position + obj.transform.localScale / 2f;
            Vector3 center = (min + max) / 2f;
            Vector3 size = max - min;

            Gizmos.DrawWireCube(center, size);
        }
    }
}
