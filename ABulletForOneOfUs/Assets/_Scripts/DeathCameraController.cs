using UnityEngine;
using System.Collections;

public class DeathCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player; // R�f�rence au joueur
    public Transform cameraTransform; // La cam�ra principale ou une cam�ra d�di�e
    public float transitionDuration = 2f; // Dur�e de la transition de la cam�ra

    [Header("Animation Settings")]
    public Animator playerAnimator; // Animations du joueur
    public string playerDeathAnimation = "Death"; // Nom de l'animation de mort du joueur
    public string[] ghostDanceAnimations = { "Shuffling", "Hiphop", "Twerk", "BreakDance" }; // Animations de danse du fant�me

    [Header("UI Settings")]
    public GameObject gameOverText; // Texte "Game Over" � afficher
    public GameObject playerUI; // R�f�rence au Canvas principal de l'UI
    public GameObject endScreen; // R�f�rence au GameObject "EndScreen"

    [Header("Sound Settings")]
    public AudioClip deathSound; // Son � jouer lorsque le joueur meurt
    public AudioSource audioSource; // Source audio pour jouer les sons

    private Animator ghostAnimator; // R�f�rence � l'Animator du fant�me
    public FirstPersonController playerController; // Contr�le du joueur
    public GunController gunController; // Contr�le du pistolet

    void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.SetActive(false); // Masquer le texte de Game Over au d�but
        }

        if (playerUI != null)
        {
            playerUI.SetActive(true); // Assurer que l'UI du joueur est activ�e au d�part
        }

        if (endScreen != null)
        {
            endScreen.SetActive(false); // Masquer l'�cran de fin au d�part
        }
    }

    public void TriggerDeathSequence()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (gunController != null)
        {
            gunController.enabled = false;
        }

        // D�sactiver tous les enfants sauf EndScreen
        if (playerUI != null && endScreen != null)
        {
            DisableAllExceptEndScreen(playerUI, endScreen);
        }

        StartCoroutine(DeathSequence());
    }

    private void DisableAllExceptEndScreen(GameObject canvas, GameObject exception)
    {
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject != exception)
            {
                child.gameObject.SetActive(false); // D�sactiver tout sauf le GameObject d'exception
            }
        }

        // Masquer l'EndScreen ici, il sera activ� plus tard
        exception.SetActive(false);
    }

    private IEnumerator DeathSequence()
    {
        // Trouver la r�f�rence au fant�me
        ghostAnimator = FindObjectOfType<GhostController>()?.GetComponent<Animator>();
        if (ghostAnimator == null)
        {
            Debug.LogWarning("Fant�me introuvable. Assurez-vous qu'un fant�me est instanci�.");
            yield break;
        }

        Transform ghost = ghostAnimator.transform;

        // Jouer l'animation de mort du joueur
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger(playerDeathAnimation);
        }

        // Jouer le son de mort
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Passer directement � la vue sur le fant�me
        Transform cameraFocusPoint = ghost.Find("CameraFocusPoint");
        if (cameraFocusPoint != null)
        {
            Vector3 ghostTargetPosition = cameraFocusPoint.position;
            Vector3 lookAtPoint = ghost.position + Vector3.up * 1.5f;
            Quaternion ghostTargetRotation = Quaternion.LookRotation(lookAtPoint - ghostTargetPosition);

            float elapsedTime = 0f;
            Vector3 startPosition = cameraTransform.position;
            Quaternion startRotation = cameraTransform.rotation;

            while (elapsedTime < transitionDuration)
            {
                cameraTransform.position = Vector3.Lerp(startPosition, ghostTargetPosition, elapsedTime / transitionDuration);
                cameraTransform.rotation = Quaternion.Slerp(startRotation, ghostTargetRotation, elapsedTime / transitionDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cameraTransform.position = ghostTargetPosition;
            cameraTransform.rotation = ghostTargetRotation;

            // Jouer une animation de danse pour le fant�me
            if (ghostAnimator != null)
            {
                int randomIndex = Random.Range(0, ghostDanceAnimations.Length);
                ghostAnimator.SetTrigger(ghostDanceAnimations[randomIndex]);
            }
        }
        else
        {
            Debug.LogWarning("CameraFocusPoint introuvable sur le fant�me.");
        }

        // Afficher le texte "Game Over" et l'EndScreen en m�me temps
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
        if (endScreen != null)
        {
            endScreen.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnRetryButtonClicked()
    {
        // Red�marre la sc�ne actuelle
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
