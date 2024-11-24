using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance; // Instance Singleton

    [Header("UI Settings")]
    public TextMeshProUGUI pumpkinCounterText; // Référence au TextMeshPro pour afficher le compteur

    [Header("Game Settings")]
    public int totalPumpkins = 0; // Nombre total de citrouilles à collecter
    private int collectedPumpkins = 0; // Nombre de citrouilles collectées

    void Awake()
    {
        // Singleton pour un accès global
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI(); // Initialiser l'UI au démarrage
    }

    public void CollectPumpkin()
    {
        collectedPumpkins++;

        // Mettre à jour l'UI
        UpdateUI();

        // Vérifier si toutes les citrouilles ont été collectées
        if (collectedPumpkins >= totalPumpkins)
        {
            Debug.Log("Toutes les citrouilles ont été collectées !");
            OnAllPumpkinsCollected();
        }
    }

    private void UpdateUI()
    {
        if (pumpkinCounterText != null)
        {
            pumpkinCounterText.text = $"{collectedPumpkins}";
        }
    }

    private void OnAllPumpkinsCollected()
    {
        // Logique quand toutes les citrouilles sont collectées
        Debug.Log("Félicitations, vous avez collecté toutes les citrouilles !");
        // Vous pouvez déclencher une animation, afficher un écran de victoire, etc.
    }
}
