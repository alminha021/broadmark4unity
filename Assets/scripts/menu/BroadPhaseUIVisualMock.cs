using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BroadPhaseUIVisualMock : MonoBehaviour
{
    public TMP_Dropdown algorithmDropdown;
    public TMP_Dropdown objectCountDropdown;
    public TMP_Dropdown scenarioDropdown;
    public TMP_Dropdown objectTypeDropdown;
    public Button startButton;

    void Start()
    {
        // Configuração dos dropdowns
        algorithmDropdown.ClearOptions();
        algorithmDropdown.AddOptions(new System.Collections.Generic.List<string> { "BruteForce", "Tracy", "KDTree"});

        objectCountDropdown.ClearOptions();
        objectCountDropdown.AddOptions(new System.Collections.Generic.List<string> { "100", "200", "400", "800"});

        scenarioDropdown.ClearOptions();
        scenarioDropdown.AddOptions(new System.Collections.Generic.List<string> { "FreeFall", "Brownian", "RandomGravity", "RotatingGravity", "Hurricane" });

        objectTypeDropdown.ClearOptions();
        objectTypeDropdown.AddOptions(new System.Collections.Generic.List<string> { "Cube", "Sphere" });

        startButton.onClick.AddListener(StartBenchmark);
    }

    void StartBenchmark()
    {
        BenchmarkConfig.Instance.algoritmo = algorithmDropdown.options[algorithmDropdown.value].text;

        int.TryParse(objectCountDropdown.options[objectCountDropdown.value].text, out int count);
        BenchmarkConfig.Instance.numeroDeObjetos = count;

        BenchmarkConfig.Instance.prefabNome = objectTypeDropdown.options[objectTypeDropdown.value].text;

        BenchmarkConfig.Instance.scenario = (PhysicsModeController.MovementMode)System.Enum.Parse(
            typeof(PhysicsModeController.MovementMode),
            scenarioDropdown.options[scenarioDropdown.value].text
        );

        Debug.Log($"Benchmark configurado: {BenchmarkConfig.Instance.algoritmo}, {BenchmarkConfig.Instance.numeroDeObjetos}, {BenchmarkConfig.Instance.prefabNome}, {BenchmarkConfig.Instance.scenario}");

        // Usa o nome real da sua cena de teste:
        SceneManager.LoadScene("browniantest");
    }
}
