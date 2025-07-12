using UnityEngine;
using System.Collections;

public class SceneObjectSpawner : MonoBehaviour //spawner de obj na scene
{
    [Header("Prefab para spawn")]
    public GameObject cubePrefab;
    public GameObject spherePrefab;

    private int numberOfObjects;
    private PhysicsModeController.MovementMode chosenMode;
    private string algoritmo;

    void Start()
    {
        // Lê o que o usuário escolheu no BenchmarkConfig
        numberOfObjects = BenchmarkConfig.Instance.numeroDeObjetos;
        chosenMode = BenchmarkConfig.Instance.scenario;
        algoritmo = BenchmarkConfig.Instance.algoritmo;

        Debug.Log($"Spawner: Objects={numberOfObjects}, Mode={chosenMode}, Algoritmo={algoritmo}");

        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        GameObject[] spawnedObjects = new GameObject[numberOfObjects];

        // Decide qual prefab usar
        GameObject prefab = BenchmarkConfig.Instance.prefabNome == "Sphere" ? spherePrefab : cubePrefab;

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

        // Aqui você pode passar o `algoritmo` para o manager de Broadphase, se tiver.
        Debug.Log($"Spawner terminou — todos objetos criados com modo {chosenMode}.");
    }
}
