using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseMenuUI; // R�f�rence au Canvas du menu de pause
    public Button resumeButton;    // Bouton Reprendre
    public Button quitButton;      // Bouton Quitter
    public Button soundButton;     // Bouton pour activer/d�sactiver le son
    public GameObject soundOnIcon; // Image "Sound_ON" (GameObject enfant)
    public GameObject soundOffIcon; // Image "Sound_Off" (GameObject enfant)

    [Header("Sound Settings")]
    public AudioSource audioSource; // Audio source pour couper/activer le son

    private bool isPaused = false;
    private bool isMuted = false;

    void Start()
    {
        // Assignation des m�thodes aux boutons
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitToMainMenu);
        soundButton.onClick.AddListener(ToggleSound);

        // Assurer que le menu de pause est cach� au d�but
        pauseMenuUI.SetActive(false);
        soundOffIcon.SetActive(false); // Assurer que l'ic�ne "Sound_Off" est cach�e au d�but
    }

    void Update()
    {
        // Activer/d�sactiver le menu de pause quand on appuie sur �chap
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

    // M�thode pour mettre le jeu en pause
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Arr�ter le temps
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; // D�verrouiller le curseur
        Cursor.visible = true; // Rendre le curseur visible
    }

    // M�thode pour reprendre le jeu
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Reprendre le temps
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked; // Verrouiller le curseur
        Cursor.visible = false; // Cacher le curseur
    }

    // M�thode pour quitter vers le menu principal
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Reprendre le temps avant de quitter
        SceneManager.LoadScene("MainMenu"); // Charger la sc�ne MainMenu (assure-toi que la sc�ne est ajout�e au Build Settings)
    }

    // M�thode pour activer/d�sactiver le son et basculer les ic�nes
    public void ToggleSound()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            AudioListener.volume = 0f; // Couper tout le son du jeu
            soundOnIcon.SetActive(false); // Cacher l'ic�ne "Sound_ON"
            soundOffIcon.SetActive(true); // Afficher l'ic�ne "Sound_Off"
        }
        else
        {
            AudioListener.volume = 1f; // Activer le son du jeu
            soundOnIcon.SetActive(true); // Afficher l'ic�ne "Sound_ON"
            soundOffIcon.SetActive(false); // Cacher l'ic�ne "Sound_Off"
        }
    }
}
