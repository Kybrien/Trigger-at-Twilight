using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance; // Singleton pour un accès global

    [Header("UI Settings")]
    public TextMeshProUGUI pumpkinCounterText; // Texte qui affiche le compteur des citrouilles
    public GameObject winScreenCanvas; // Canvas affiché en cas de victoire

    [Header("Game Settings")]
    public int totalPumpkins = 5; // Nombre total de citrouilles à collecter
    private int collectedPumpkins = 0; // Nombre de citrouilles collectées

    [Header("Audio Settings")]
    public AudioClip winSound; // Son de victoire
    public AudioSource audioSource; // Source audio pour jouer le son

    private bool hasWon = false; // Indique si la victoire a déjà été atteinte

    void Awake()
    {
        // Configuration du Singleton
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
        // Initialisation de l'UI et des éléments liés à la victoire
        UpdateUI();

        if (winScreenCanvas != null)
        {
            winScreenCanvas.SetActive(false); // Masque l'écran de victoire au démarrage
        }
    }

    public void CollectPumpkin()
    {
        if (hasWon) return; // Ne rien faire si la victoire a déjà été atteinte

        collectedPumpkins++;
        UpdateUI();

        // Vérifie si toutes les citrouilles ont été collectées
        if (collectedPumpkins >= totalPumpkins)
        {
            TriggerWinCondition();
        }
    }

    private void UpdateUI()
    {
        if (pumpkinCounterText != null)
        {
            pumpkinCounterText.text = $"{collectedPumpkins}/{totalPumpkins} Pumpkins Collected";
        }
    }

    private void TriggerWinCondition()
    {
        hasWon = true;

        // Arrêter le jeu et désactiver les mécaniques
        StopGame();

        // Jouer le son de victoire
        if (audioSource != null && winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }

        // Afficher le canvas de victoire
        if (winScreenCanvas != null)
        {
            winScreenCanvas.SetActive(true);
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

        // Arrêter les mécaniques liées au fantôme
        GhostManager ghostManager = FindObjectOfType<GhostManager>();
        if (ghostManager != null)
        {
            CancelInvoke(); // Stop tous les Invoke
        }

        // Optionnel : Arrêter le temps
        Time.timeScale = 0f;
    }
}
