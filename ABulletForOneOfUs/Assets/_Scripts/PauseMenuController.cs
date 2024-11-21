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
    private float inputCooldown = 0.5f; // Cooldown de 500 ms pour �viter les doubles clics
    private float lastInputTime;
    private bool interactionBlocked = false; // Bloquer les interactions durant le cooldown

    private FirstPersonController playerController;
    private GunController gunController;

    void Start()
    {
        // Assignation des m�thodes aux boutons
        resumeButton.onClick.AddListener(() => { if (!interactionBlocked) ResumeGame(); });
        quitButton.onClick.AddListener(() => { if (!interactionBlocked) QuitToMainMenu(); });
        soundButton.onClick.AddListener(() => { if (!interactionBlocked) ToggleSound(); });

        // Assurer que le menu de pause est cach� au d�but
        pauseMenuUI.SetActive(false);
        soundOffIcon.SetActive(false); // Assurer que l'ic�ne "Sound_Off" est cach�e au d�but
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Obtenir les r�f�rences des contr�leurs du joueur et du pistolet
        playerController = FindObjectOfType<FirstPersonController>();
        gunController = FindObjectOfType<GunController>();
    }

    void Update()
    {
        // Activer/d�sactiver le menu de pause quand on appuie sur �chap
        if (Input.GetKeyDown(KeyCode.Escape) && Time.unscaledTime - lastInputTime > inputCooldown)
        {
            lastInputTime = Time.unscaledTime;
            interactionBlocked = true;
            Invoke(nameof(UnblockInteraction), inputCooldown);

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

        // D�verrouiller et rendre le curseur visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // D�sactiver le script de contr�le du joueur
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // D�sactiver le contr�le du pistolet
        if (gunController != null)
        {
            gunController.enabled = false;
        }
    }

    // M�thode pour reprendre le jeu
    public void ResumeGame()
    {
        Debug.Log("Resume");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Reprendre le temps
        isPaused = false;

        // Verrouiller le curseur et le rendre invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // R�activer le script de contr�le du joueur
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // R�activer le contr�le du pistolet
        if (gunController != null)
        {
            gunController.enabled = true;
        }
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
            Debug.Log("Sound OFF");
            AudioListener.volume = 0f; // Couper tout le son du jeu
            soundOnIcon.SetActive(false); // Cacher l'ic�ne "Sound_ON"
            soundOffIcon.SetActive(true); // Afficher l'ic�ne "Sound_Off"
        }
        else
        {
            Debug.Log("Sound ON");
            AudioListener.volume = 1f; // Activer le son du jeu
            soundOnIcon.SetActive(true); // Afficher l'ic�ne "Sound_ON"
            soundOffIcon.SetActive(false); // Cacher l'ic�ne "Sound_Off"
        }
    }

    // M�thode pour d�bloquer les interactions apr�s le cooldown
    private void UnblockInteraction()
    {
        interactionBlocked = false;
    }
}
