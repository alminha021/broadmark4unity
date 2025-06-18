using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BroadPhaseUIVisualMock : MonoBehaviour
{
    public TMP_Dropdown algorithmDropdown;
    public TMP_Dropdown objectCountDropdown;
    public TMP_Dropdown scenarioDropdown;
    public TMP_Dropdown objectTypeDropdown;
    public Button startButton;

    void Start()
    {
        algorithmDropdown.ClearOptions();
        algorithmDropdown.AddOptions(new System.Collections.Generic.List<string> { "Brute Force", "KDTree" });

        objectCountDropdown.ClearOptions();
        objectCountDropdown.AddOptions(new System.Collections.Generic.List<string> { "10", "20", "50", "100" });

        scenarioDropdown.ClearOptions();
        scenarioDropdown.AddOptions(new System.Collections.Generic.List<string> { "Free Fall", "Brownian", "Gravity", "Crowd Sim" });

        objectTypeDropdown.ClearOptions();
        objectTypeDropdown.AddOptions(new System.Collections.Generic.List<string> { "Cubo", "Círculo" });

        startButton.onClick.AddListener(() =>
        {
            Debug.Log("Botão Start clicado! (Somente visual, sem simulação)");
        });
    }
}
