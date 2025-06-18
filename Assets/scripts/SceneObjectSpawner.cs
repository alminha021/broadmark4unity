using UnityEngine;

public class SceneObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int numberOfObjects = 20;

    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-40f, 40f),
                Random.Range(-40f, 40f),
                Random.Range(-40f, 40f)
            );

            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.transform.localScale = Vector3.one * Random.Range(3f, 6f);
            obj.AddComponent<AABBObjectController>();
        }
    }
}
