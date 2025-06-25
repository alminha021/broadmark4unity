using UnityEngine;

public class WorldBoundsEdgesRenderer : MonoBehaviour
{
    public Vector3 worldMin = new Vector3(-50, -50, -50);
    public Vector3 worldMax = new Vector3(50, 50, 50);
    public float edgeThickness = 0.2f;
    public Material edgeMaterial;

    void Start()
    {
        Vector3[] corners = new Vector3[8];
        corners[0] = new Vector3(worldMin.x, worldMin.y, worldMin.z); // 0
        corners[1] = new Vector3(worldMax.x, worldMin.y, worldMin.z); // 1
        corners[2] = new Vector3(worldMax.x, worldMin.y, worldMax.z); // 2
        corners[3] = new Vector3(worldMin.x, worldMin.y, worldMax.z); // 3
        corners[4] = new Vector3(worldMin.x, worldMax.y, worldMin.z); // 4
        corners[5] = new Vector3(worldMax.x, worldMax.y, worldMin.z); // 5
        corners[6] = new Vector3(worldMax.x, worldMax.y, worldMax.z); // 6
        corners[7] = new Vector3(worldMin.x, worldMax.y, worldMax.z); // 7

        CreateEdge(corners[0], corners[1]);
        CreateEdge(corners[1], corners[2]);
        CreateEdge(corners[2], corners[3]);
        CreateEdge(corners[3], corners[0]);

        CreateEdge(corners[4], corners[5]);
        CreateEdge(corners[5], corners[6]);
        CreateEdge(corners[6], corners[7]);
        CreateEdge(corners[7], corners[4]);

        CreateEdge(corners[0], corners[4]);
        CreateEdge(corners[1], corners[5]);
        CreateEdge(corners[2], corners[6]);
        CreateEdge(corners[3], corners[7]);
    }

    void CreateEdge(Vector3 start, Vector3 end)
    {
        Vector3 center = (start + end) / 2f;
        Vector3 dir = end - start;
        float length = dir.magnitude;

        GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Cube);
        edge.name = "Edge";
        edge.transform.SetParent(transform);
        edge.transform.position = center;
        edge.transform.rotation = Quaternion.LookRotation(dir);
        edge.transform.localScale = new Vector3(edgeThickness, edgeThickness, length);

        if (edgeMaterial != null)
            edge.GetComponent<Renderer>().material = edgeMaterial;

        Destroy(edge.GetComponent<Collider>()); // remove collider se não precisar
    }
}
