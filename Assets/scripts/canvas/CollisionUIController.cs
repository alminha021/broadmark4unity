using TMPro; // se estiver usando TextMeshPro
using UnityEngine;
using UnityEngine.UI; // se estiver usando o Text normal

public class CollisionUIController : MonoBehaviour
{
    public TextMeshProUGUI collisionText; // ou Text se não for TMP

    private ulong currentCollisions = 0;

    public void UpdateCollisionCount(ulong count)
    {
        currentCollisions = count;
        collisionText.text = $"Possíveis Colisões: {currentCollisions}";
    }
}
