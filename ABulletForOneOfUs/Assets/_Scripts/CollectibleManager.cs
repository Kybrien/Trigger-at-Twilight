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

    [Header("Camera and Player Settings")]
    public GameObject player; // Le joueur à désactiver
    public GameObject endingCamera; // Caméra ou GameObject à activer en cas de victoire

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

        if (endingCamera != null)
        {
            endingCamera.SetActive(false); // Assure que la caméra de fin est désactivée au démarrage
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
            pumpkinCounterText.text = $"{collectedPumpkins}";
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

        // Activer la caméra de fin
        if (endingCamera != null)
        {
            endingCamera.SetActive(true);
        }
    }

    private void StopGame()
    {
        // Désactiver le joueur
        if (player != null)
        {
            player.SetActive(false);
        }

        // Arrêter les mécaniques liées au fantôme
        GhostManager ghostManager = FindObjectOfType<GhostManager>();
        if (ghostManager != null)
        {
            CancelInvoke(); // Stop tous les Invoke
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
