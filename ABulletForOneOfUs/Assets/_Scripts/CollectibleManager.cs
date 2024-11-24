using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance; // Instance Singleton

    [Header("UI Settings")]
    public TextMeshProUGUI pumpkinCounterText; // R�f�rence au TextMeshPro pour afficher le compteur
    public Canvas winCanvas; // Canvas � afficher � la victoire

    [Header("Game Settings")]
    public int totalPumpkins = 5; // Nombre total de citrouilles � collecter
    private int collectedPumpkins = 0; // Nombre de citrouilles collect�es

    [Header("Audio Settings")]
    public AudioClip winSound; // Son de victoire
    public AudioSource audioSource; // Source audio pour jouer les sons

    private bool hasWon = false; // Indique si la condition de victoire est atteinte

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

        if (winCanvas != null)
        {
            winCanvas.gameObject.SetActive(false); // Masquer le canvas de victoire au d�part
        }
    }

    public void CollectPumpkin()
    {
        if (hasWon) return; // Si la victoire est d�j� atteinte, ne rien faire

        collectedPumpkins++;

        // Mettre � jour l'UI
        UpdateUI();

        // V�rifier si toutes les citrouilles ont �t� collect�es
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

        // D�sactiver tous les autres Canvas
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

        // D�sactiver les m�caniques de jeu
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
        // D�sactiver les inputs du joueur
        FirstPersonController playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Arr�ter les timers et le spawn du fant�me
        GhostManager ghostManager = FindObjectOfType<GhostManager>();
        if (ghostManager != null)
        {
            CancelInvoke(); // Annule tous les Invoke en cours
        }

        // D�sactiver tout comportement li� au joueur
        Time.timeScale = 0f; // Optionnel : Arr�ter le temps
    }
}
