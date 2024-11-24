using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance; // Instance Singleton

    [Header("UI Settings")]
    public TextMeshProUGUI pumpkinCounterText; // R�f�rence au TextMeshPro pour afficher le compteur

    [Header("Game Settings")]
    public int totalPumpkins = 0; // Nombre total de citrouilles � collecter
    private int collectedPumpkins = 0; // Nombre de citrouilles collect�es

    void Awake()
    {
        // Singleton pour un acc�s global
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
        UpdateUI(); // Initialiser l'UI au d�marrage
    }

    public void CollectPumpkin()
    {
        collectedPumpkins++;

        // Mettre � jour l'UI
        UpdateUI();

        // V�rifier si toutes les citrouilles ont �t� collect�es
        if (collectedPumpkins >= totalPumpkins)
        {
            Debug.Log("Toutes les citrouilles ont �t� collect�es !");
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
        // Logique quand toutes les citrouilles sont collect�es
        Debug.Log("F�licitations, vous avez collect� toutes les citrouilles !");
        // Vous pouvez d�clencher une animation, afficher un �cran de victoire, etc.
    }
}
