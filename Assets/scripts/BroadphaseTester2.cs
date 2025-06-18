using UnityEngine;

public class BroadphaseTester2 : MonoBehaviour {
    public GameObject[] objects;

    void Start() {
        Debug.Log("Chamando DLL nova...");
        int result = BroadPhaseDLL.HelloSum(5, 7);
        Debug.Log("Resultado som bftester2: " + result);

        BroadPhaseDLL.InitBF(objects.Length);
    }

    void Update() {
        for (int i = 0; i < objects.Length; i++) {
            Bounds bounds = objects[i].GetComponent<Collider>().bounds;
            BroadPhaseDLL.UpdateAABB(i,
                bounds.min.x, bounds.min.y, bounds.min.z,
                bounds.max.x, bounds.max.y, bounds.max.z);
        }

        BroadPhaseDLL.RunBF();

        int count = BroadPhaseDLL.GetOverlapCount();
        for (int i = 0; i < count; i++) {
            BroadPhaseDLL.GetOverlapPair(i, out int idA, out int idB);
            Debug.Log($"Overlap: {idA} x {idB}");
        }
    }
}
