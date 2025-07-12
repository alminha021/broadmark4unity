using UnityEngine;

public class AlgorithmManager : MonoBehaviour
{
    public GameObject algorithm1;
    public GameObject algorithm2;
    public GameObject algorithm3;

    public enum AlgorithmType { Alg1, Alg2, Alg3 }
    public AlgorithmType selectedAlgorithm = AlgorithmType.Alg1;

    void Awake()
    {
        // Lê o nome salvo no BenchmarkConfig
        string algName = BenchmarkConfig.Instance.algoritmo;

        // Converte o nome em enum — faz um map simples
        switch (algName)
        {
            case "BruteForce":
                selectedAlgorithm = AlgorithmType.Alg1;
                break;
            case "Tracy":
                selectedAlgorithm = AlgorithmType.Alg2;
                break;
            case "KDTree":
                selectedAlgorithm = AlgorithmType.Alg3;
                break;
            default:
                selectedAlgorithm = AlgorithmType.Alg1; // fallback
                break;
        }

        Debug.Log($"🔍 AlgorithmManager ativando algoritmo: {selectedAlgorithm}");

        SetActiveAlgorithm();
    }

    void SetActiveAlgorithm()
    {
        if (algorithm1 != null)
            algorithm1.SetActive(selectedAlgorithm == AlgorithmType.Alg1);
        if (algorithm2 != null)
            algorithm2.SetActive(selectedAlgorithm == AlgorithmType.Alg2);
        if (algorithm3 != null)
            algorithm3.SetActive(selectedAlgorithm == AlgorithmType.Alg3);
    }
}
