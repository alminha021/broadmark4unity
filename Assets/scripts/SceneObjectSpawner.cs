using UnityEngine;
using System.Collections;

public class SceneObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int numberOfObjects = 20;

    [Header("Physics Mode para os objetos")]
    public PhysicsModeController.MovementMode chosenMode = PhysicsModeController.MovementMode.Brownian;

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        GameObject[] spawnedObjects = new GameObject[numberOfObjects];

        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-40f, 40f),
                Random.Range(-40f, 40f),
                Random.Range(-40f, 40f)
            );

            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.transform.localScale = Vector3.one * Random.Range(2f, 4f);

            if (obj.GetComponent<AABBObjectController>() == null)
                obj.AddComponent<AABBObjectController>();

            spawnedObjects[i] = obj;
        }

        yield return null;

        foreach (var obj in spawnedObjects)
        {
            PhysicsModeController phys = obj.GetComponent<PhysicsModeController>();
            if (phys == null)
                phys = obj.AddComponent<PhysicsModeController>();

            phys.SetMode(chosenMode);
        }
    }
}
