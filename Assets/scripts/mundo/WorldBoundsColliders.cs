using UnityEngine;

public class WorldBoundsColliders : MonoBehaviour
{
    public Vector3 worldMin = new Vector3(-50, -50, -50);
    public Vector3 worldMax = new Vector3(50, 50, 50);
    public float wallThickness = 1f;

    void Start()
    {
        Vector3 size = worldMax - worldMin;
        Vector3 center = (worldMin + worldMax) / 2f;

        // Base (Y-)
        CreateWall("Bottom",
            new Vector3(center.x, worldMin.y - wallThickness / 2f, center.z),
            new Vector3(size.x, wallThickness, size.z));

        // Topo (Y+)
        CreateWall("Top",
            new Vector3(center.x, worldMax.y + wallThickness / 2f, center.z),
            new Vector3(size.x, wallThickness, size.z));

        // Frente (Z+)
        CreateWall("Front",
            new Vector3(center.x, center.y, worldMax.z + wallThickness / 2f),
            new Vector3(size.x, size.y, wallThickness));

        // Trás (Z-)
        CreateWall("Back",
            new Vector3(center.x, center.y, worldMin.z - wallThickness / 2f),
            new Vector3(size.x, size.y, wallThickness));

        // Esquerda (X-)
        CreateWall("Left",
            new Vector3(worldMin.x - wallThickness / 2f, center.y, center.z),
            new Vector3(wallThickness, size.y, size.z));

        // Direita (X+)
        CreateWall("Right",
            new Vector3(worldMax.x + wallThickness / 2f, center.y, center.z),
            new Vector3(wallThickness, size.y, size.z));
    }

    void CreateWall(string name, Vector3 position, Vector3 size)
    {
        GameObject wall = new GameObject(name);
        wall.transform.parent = transform;
        wall.transform.position = position;

        BoxCollider collider = wall.AddComponent<BoxCollider>();
        collider.size = size;

        // (Opcional) Visual debug
        // wall.AddComponent<MeshRenderer>().material = debugMaterial;
        // wall.AddComponent<MeshFilter>().mesh = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>().sharedMesh;
    }
}
