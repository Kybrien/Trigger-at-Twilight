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
    }

    void Update()
    {
        // Activer/désactiver le menu de pause quand on appuie sur Échap
        if (Input.GetButtonDown("Pause"))
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
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Arrêter le temps
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; // Déverrouiller le curseur
        Cursor.visible = true; // Rendre le curseur visible
        // Désactiver le script de contrôle du joueur
        FirstPersonController playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        else
        {
            Debug.Log("Pas trouve le FP controller dsl mon reuf");
        }
    }

    // Méthode pour reprendre le jeu
    public void ResumeGame()
    {
        Debug.Log("Resume btn clicked");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Reprendre le temps
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked; // Verrouiller le curseur
        Cursor.visible = false; // Cacher le curseur
        // Réactiver le script de contrôle du joueur
        FirstPersonController playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    // Méthode pour quitter vers le menu principal
    public void QuitToMainMenu()
    {
        Debug.Log("Quit to Main Menu");
        Time.timeScale = 1f; // Reprendre le temps avant de quitter
        SceneManager.LoadScene("MainMenu"); // Charger la scène MainMenu (assure-toi que la scène est ajoutée au Build Settings)
    }

    // Méthode pour activer/désactiver le son et basculer les icônes
    public void ToggleSound()
    {
        Debug.Log("Sound Btn clicked");
        isMuted = !isMuted;

        if (isMuted)
        {
            Debug.Log("Sound OFF");
            AudioListener.volume = 0f; // Couper tout le son du jeu
            soundOnIcon.SetActive(false); // Cacher l'icône "Sound_ON"
            soundOffIcon.SetActive(true); // Afficher l'icône "Sound_Off"
        }
        else
        {
            Debug.Log("On");
            AudioListener.volume = 1f; // Activer le son du jeu
            soundOnIcon.SetActive(true); // Afficher l'icône "Sound_ON"
            soundOffIcon.SetActive(false); // Cacher l'icône "Sound_Off"
        }
    }
}
