using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseMenuUI; // Référence au Canvas du menu de pause
    public Button resumeButton;    // Bouton Reprendre
    public Button quitButton;      // Bouton Quitter
    public Button soundButton;     // Bouton pour activer/désactiver le son
    public GameObject soundOnIcon; // Image "Sound_ON" (GameObject enfant)
    public GameObject soundOffIcon; // Image "Sound_Off" (GameObject enfant)

    [Header("Sound Settings")]
    public AudioSource audioSource; // Audio source pour couper/activer le son

    private bool isPaused = false;
    private bool isMuted = false;

    private FirstPersonController playerController;
    private GunController gunController;

    void Start()
    {
        // Assignation des méthodes aux boutons
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitToMainMenu);
        soundButton.onClick.AddListener(ToggleSound);

        // Assurer que le menu de pause est caché au début
        pauseMenuUI.SetActive(false);
        soundOffIcon.SetActive(false); // Assurer que l'icône "Sound_Off" est cachée au début
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Obtenir les références des contrôleurs du joueur et du pistolet
        playerController = FindObjectOfType<FirstPersonController>();
        gunController = FindObjectOfType<GunController>();
    }

    void Update()
    {
        // Activer/désactiver le menu de pause quand on appuie sur Échap
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Méthode pour mettre le jeu en pause
    public void PauseGame()
    {
        Debug.Log("PAUSED");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Arrêter le temps
        isPaused = true;

        // Déverrouiller et rendre le curseur visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Désactiver le script de contrôle du joueur
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Désactiver le contrôle du pistolet
        if (gunController != null)
        {
            gunController.enabled = false;
        }
    }

    // Méthode pour reprendre le jeu
    public void ResumeGame()
    {
        Debug.Log("RESUME");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Reprendre le temps
        isPaused = false;

        // Verrouiller le curseur et le rendre invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Réactiver le script de contrôle du joueur
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Réactiver le contrôle du pistolet
        if (gunController != null)
        {
            gunController.enabled = true;
        }
    }

    // Méthode pour quitter vers le menu principal
    public void QuitToMainMenu()
    {
        Debug.Log("LEAVE");
        Time.timeScale = 1f; // Reprendre le temps avant de quitter
        SceneManager.LoadScene("MainMenu"); // Charger la scène MainMenu (assure-toi que la scène est ajoutée au Build Settings)
    }

    // Méthode pour activer/désactiver le son et basculer les icônes
    public void ToggleSound()
    {
        Debug.Log("SOUND");
        isMuted = !isMuted;

        if (isMuted)
        {
            AudioListener.volume = 0f; // Couper tout le son du jeu
            soundOnIcon.SetActive(false); // Cacher l'icône "Sound_ON"
            soundOffIcon.SetActive(true); // Afficher l'icône "Sound_Off"
        }
        else
        {
            AudioListener.volume = 1f; // Activer le son du jeu
            soundOnIcon.SetActive(true); // Afficher l'icône "Sound_ON"
            soundOffIcon.SetActive(false); // Cacher l'icône "Sound_Off"
        }
    }
}
