using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance; // Singleton pour un acc�s global

    [Header("UI Settings")]
    public TextMeshProUGUI pumpkinCounterText; // Texte qui affiche le compteur des citrouilles
    public GameObject winScreenCanvas; // Canvas affich� en cas de victoire

    [Header("Game Settings")]
    public int totalPumpkins = 5; // Nombre total de citrouilles � collecter
    private int collectedPumpkins = 0; // Nombre de citrouilles collect�es

    [Header("Audio Settings")]
    public AudioClip winSound; // Son de victoire
    public AudioSource audioSource; // Source audio pour jouer le son

    private bool hasWon = false; // Indique si la victoire a d�j� �t� atteinte

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
        // Initialisation de l'UI et des �l�ments li�s � la victoire
        UpdateUI();

        if (winScreenCanvas != null)
        {
            winScreenCanvas.SetActive(false); // Masque l'�cran de victoire au d�marrage
        }
    }

    public void CollectPumpkin()
    {
        if (hasWon) return; // Ne rien faire si la victoire a d�j� �t� atteinte

        collectedPumpkins++;
        UpdateUI();

        // V�rifie si toutes les citrouilles ont �t� collect�es
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

        // Arr�ter le jeu et d�sactiver les m�caniques
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
        // D�sactiver les inputs du joueur
        FirstPersonController playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Arr�ter les m�caniques li�es au fant�me
        GhostManager ghostManager = FindObjectOfType<GhostManager>();
        if (ghostManager != null)
        {
            CancelInvoke(); // Stop tous les Invoke
        }

        // Optionnel : Arr�ter le temps
        Time.timeScale = 0f;
    }
}
