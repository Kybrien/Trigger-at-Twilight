using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance; // Instance Singleton

    [Header("UI Settings")]
    public TextMeshProUGUI pumpkinCounterText; // Référence au TextMeshPro pour afficher le compteur
    public Canvas winCanvas; // Canvas à afficher à la victoire

    [Header("Game Settings")]
    public int totalPumpkins = 5; // Nombre total de citrouilles à collecter
    private int collectedPumpkins = 0; // Nombre de citrouilles collectées

    [Header("Audio Settings")]
    public AudioClip winSound; // Son de victoire
    public AudioSource audioSource; // Source audio pour jouer les sons

    private bool hasWon = false; // Indique si la condition de victoire est atteinte

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

        if (winCanvas != null)
        {
            winCanvas.gameObject.SetActive(false); // Masquer le canvas de victoire au départ
        }
    }

    public void CollectPumpkin()
    {
        if (hasWon) return; // Si la victoire est déjà atteinte, ne rien faire

        collectedPumpkins++;

        // Mettre à jour l'UI
        UpdateUI();

        // Vérifier si toutes les citrouilles ont été collectées
        if (collectedPumpkins >= totalPumpkins)
        {
            TriggerWinCondition();
        }
    }

    private void UpdateUI()
    {
        if (pumpkinCounterText != null)
        {
            pumpkinCounterText.text = $"{collectedPumpkins}";
        }
    }

    private void TriggerWinCondition()
    {
        hasWon = true;

        // Désactiver tous les autres Canvas
        DisableAllCanvases();

        // Jouer le son de victoire
        if (audioSource != null && winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }

        // Afficher le Canvas de victoire
        if (winCanvas != null)
        {
            winCanvas.gameObject.SetActive(true);
        }

        // Désactiver les mécaniques de jeu
        StopGame();
    }

    private void DisableAllCanvases()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas != winCanvas)
            {
                canvas.gameObject.SetActive(false);
            }
        }
    }

    private void StopGame()
    {
        // Désactiver les inputs du joueur
        FirstPersonController playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Arrêter les timers et le spawn du fantôme
        GhostManager ghostManager = FindObjectOfType<GhostManager>();
        if (ghostManager != null)
        {
            CancelInvoke(); // Annule tous les Invoke en cours
        }

        // Désactiver tout comportement lié au joueur
        Time.timeScale = 0f; // Optionnel : Arrêter le temps
    }
}
