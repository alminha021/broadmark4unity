using UnityEngine;

public class BenchmarkConfig : MonoBehaviour
{//para funcionamento do singleton
    public static BenchmarkConfig Instance;

    [Header("Config Selecionada")]
    public string algoritmo = "KDTree";
    public int numeroDeObjetos = 200;
    public string prefabNome = "Cube";
    public PhysicsModeController.MovementMode scenario = PhysicsModeController.MovementMode.Brownian;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
