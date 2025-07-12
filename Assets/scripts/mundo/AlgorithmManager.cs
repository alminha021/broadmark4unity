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
        // Ativa o algoritmo escolhido e desativa os outros
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
