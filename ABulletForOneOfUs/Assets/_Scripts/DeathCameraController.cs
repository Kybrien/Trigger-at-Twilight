using UnityEngine;
using System.Collections;

public class DeathCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player; // La r�f�rence au joueur
    public Transform cameraTransform; // La cam�ra principale ou une cam�ra d�di�e aux cin�matiques
    public float transitionDuration = 2f; // Dur�e de la transition de la cam�ra
    public Vector3 cameraOffset; // Offset pour la position de la cam�ra en vue TPS

    [Header("Animation Settings")]
    public Animator playerAnimator; // Animations du joueur
    public string playerDeathAnimation = "Death"; // Nom de l'animation de mort du joueur
    public string[] ghostDanceAnimations = { "Shuffling", "Hiphop", "Twerk", "BreakDance" }; // Animations de danse du fant�me

    [Header("UI Settings")]
    public GameObject gameOverText; // Texte "Game Over" � afficher

    [Header("Sound Settings")]
    public AudioClip deathSound; // Son � jouer lorsque le joueur meurt
    public AudioSource audioSource; // Source audio pour jouer les sons

    private Animator ghostAnimator; // R�f�rence � l'Animator du fant�me
    private FirstPersonController playerController;
    private GunController gunController;

    void Start()
    {
        playerController = FindObjectOfType<FirstPersonController>();
        gunController = FindObjectOfType<GunController>();
        if (playerController != null )
        {
            Debug.Log("FPController not found");
        }
        if (gunController != null)
        {
            Debug.Log("GunController not found");
        }

        if (gameOverText != null)
        {
            gameOverText.SetActive(false); // Masquer le texte de Game Over au d�but
        }
    }

    public void TriggerDeathSequence()
    {
        
        if (playerController != null)
        {
            playerController.enabled = false;
            Debug.Log("PlayerController disabled");
        }
        if (gunController != null)
        {
            gunController.enabled = false;
            Debug.Log("GunContr0ller disabled");
        }
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
            Debug.Log("PlayerController disabled");
        }
        if (gunController != null)
        {
            gunController.enabled = false;
            Debug.Log("GunContr0ller disabled");
        }

        // Trouver la r�f�rence au fant�me nouvellement instanci�
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

        // Attendre un instant pour que le joueur commence � tomber
        yield return new WaitForSeconds(0.2f);

        // Placer la cam�ra en face du joueur pour montrer sa mort
        Vector3 targetPosition = player.position + cameraOffset;
        Quaternion targetRotation = Quaternion.LookRotation(player.position - cameraTransform.position);

        float elapsedTime = 0f;
        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;

        // Lissage de la transition de la cam�ra vers la position du joueur
        while (elapsedTime < transitionDuration)
        {
            cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / transitionDuration);
            cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = targetPosition;
        cameraTransform.rotation = targetRotation;

        // Attendre un instant pour regarder le joueur tomber
        yield return new WaitForSeconds(1f);

        // Passer � la vue sur le fant�me
        if (ghost != null)
        {
            Vector3 ghostTargetPosition = ghost.position + cameraOffset;
            Quaternion ghostTargetRotation = Quaternion.LookRotation(ghost.position - cameraTransform.position);

            elapsedTime = 0f;
            startPosition = cameraTransform.position;
            startRotation = cameraTransform.rotation;

            // Transition de la cam�ra vers la vue sur le fant�me
            while (elapsedTime < transitionDuration)
            {
                cameraTransform.position = Vector3.Lerp(startPosition, ghostTargetPosition, elapsedTime / transitionDuration);
                cameraTransform.rotation = Quaternion.Slerp(startRotation, ghostTargetRotation, elapsedTime / transitionDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cameraTransform.position = ghostTargetPosition;
            cameraTransform.rotation = ghostTargetRotation;

            // Choisir al�atoirement une animation de danse pour le fant�me
            if (ghostAnimator != null)
            {
                int randomIndex = Random.Range(0, ghostDanceAnimations.Length);
                ghostAnimator.SetTrigger(ghostDanceAnimations[randomIndex]);
            }
        }

        // Afficher le texte "Game Over"
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
    }
}
