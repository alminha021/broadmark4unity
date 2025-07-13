using UnityEngine;
using System.Collections;

public class SceneObjectSpawner : MonoBehaviour
{
    [Header("Prefabs fixos")]
    public GameObject cubePrefab;
    public GameObject spherePrefab;
    //suporte a random objects
    [Header("Prefabs para RandomObjects")]
    public GameObject[] randomPrefabs;

    private int numberOfObjects;
    private PhysicsModeController.MovementMode chosenMode;
    private string algoritmo;
    private string prefabEscolhido;

    void Start()
    {
        numberOfObjects = BenchmarkConfig.Instance.numeroDeObjetos;
        chosenMode = BenchmarkConfig.Instance.scenario;
        algoritmo = BenchmarkConfig.Instance.algoritmo;
        prefabEscolhido = BenchmarkConfig.Instance.prefabNome;

        Debug.Log($"Spawner: Objects={numberOfObjects}, Mode={chosenMode}, Algoritmo={algoritmo}, Prefab={prefabEscolhido}");

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

            GameObject prefabToSpawn;

            if (prefabEscolhido == "Cube")
            {
                prefabToSpawn = cubePrefab;
            }
            else if (prefabEscolhido == "Sphere")
            {
                prefabToSpawn = spherePrefab;
            }
            else if (prefabEscolhido == "RandomObjects")
            {
                prefabToSpawn = randomPrefabs[Random.Range(0, randomPrefabs.Length)];
            }
            else
            {
                Debug.LogWarning("⚠️ Prefab não reconhecido, usando Cube como padrão.");
                prefabToSpawn = cubePrefab;
            }

            GameObject obj = Instantiate(prefabToSpawn, pos, Quaternion.identity);
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

        Debug.Log($" Spawner terminou — todos objetos criados com modo {chosenMode}.");
    }
}
