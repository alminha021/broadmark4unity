using UnityEngine;
using System.Collections.Generic;

public class BrownianSceneSetup : MonoBehaviour
{
    [Header("Bounding Box")]
    public Vector3 boundsSize = new Vector3(5, 5, 5);
    public int objectCount = 60;
    public float objectSpeed = 2f;
    public float directionChangeInterval = 2f;
    public PrimitiveType shapeType = PrimitiveType.Cube;

    private List<GameObject> movingObjects = new List<GameObject>();

    void Start()
    {
        CreateRoom();
        SpawnMovingObjects();
    }

    void CreateRoom()
    {
        Vector3 half = boundsSize / 2f;
        float thickness = 0.1f;

        // Chão
        CreateWall(Vector3.down * half.y, new Vector3(boundsSize.x, thickness, boundsSize.z));
        // Teto
        CreateWall(Vector3.up * half.y, new Vector3(boundsSize.x, thickness, boundsSize.z));
        // Frente
        CreateWall(Vector3.forward * half.z, new Vector3(boundsSize.x, boundsSize.y, thickness));
        // Trás
        CreateWall(Vector3.back * half.z, new Vector3(boundsSize.x, boundsSize.y, thickness));
        // Direita
        CreateWall(Vector3.right * half.x, new Vector3(thickness, boundsSize.y, boundsSize.z));
        // Esquerda
        CreateWall(Vector3.left * half.x, new Vector3(thickness, boundsSize.y, boundsSize.z));
    }

    void CreateWall(Vector3 localPos, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = localPos;
        wall.transform.localScale = scale;
        wall.name = "Wall";
        wall.GetComponent<Renderer>().material.color = Color.gray;
        wall.isStatic = true;
    }

    void SpawnMovingObjects()
    {
        for (int i = 0; i < 200; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(shapeType);
            obj.transform.position = GetRandomPositionInsideBounds();
            obj.transform.localScale = Vector3.one * 0.5f;
            obj.name = "MovingObject_" + i;

            BrownianMotion mover = obj.AddComponent<BrownianMotion>();
            mover.boundsSize = boundsSize;
            mover.speed = objectSpeed;
            mover.directionChangeInterval = directionChangeInterval;

            movingObjects.Add(obj);
        }
    }

    Vector3 GetRandomPositionInsideBounds()
    {
        return new Vector3(
            Random.Range(-boundsSize.x / 2f, boundsSize.x / 2f),
            Random.Range(-boundsSize.y / 2f, boundsSize.y / 2f),
            Random.Range(-boundsSize.z / 2f, boundsSize.z / 2f)
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, boundsSize);
    }
}
