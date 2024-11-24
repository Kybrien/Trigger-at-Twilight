using UnityEngine;
using System.Collections;

public class DeathCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player; // Référence au joueur
    public Transform cameraTransform; // La caméra principale ou une caméra dédiée
    public float transitionDuration = 2f; // Durée de la transition de la caméra

    [Header("Animation Settings")]
    public Animator playerAnimator; // Animations du joueur
    public string playerDeathAnimation = "Death"; // Nom de l'animation de mort du joueur
    public string[] ghostDanceAnimations = { "Shuffling", "Hiphop", "Twerk", "BreakDance" }; // Animations de danse du fantôme

    [Header("UI Settings")]
    public GameObject gameOverText; // Texte "Game Over" à afficher

    [Header("Sound Settings")]
    public AudioClip deathSound; // Son à jouer lorsque le joueur meurt
    public AudioSource audioSource; // Source audio pour jouer les sons

    private Animator ghostAnimator; // Référence à l'Animator du fantôme
    public FirstPersonController playerController; // Contrôle du joueur
    public GunController gunController; // Contrôle du pistolet

    void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.SetActive(false); // Masquer le texte de Game Over au début
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
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Trouver la référence au fantôme
        ghostAnimator = FindObjectOfType<GhostController>()?.GetComponent<Animator>();
        if (ghostAnimator == null)
        {
            Debug.LogWarning("Fantôme introuvable. Assurez-vous qu'un fantôme est instancié.");
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

        // Attendre un instant pour que l'animation commence
        yield return new WaitForSeconds(0.5f);

        // Déplacer la caméra vers le joueur pour montrer sa mort
        Vector3 targetPosition = player.position + new Vector3(0, 2, -3); // Position derrière et légèrement au-dessus du joueur
        Quaternion targetRotation = Quaternion.LookRotation(player.position - cameraTransform.position);

        float elapsedTime = 0f;
        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;

        while (elapsedTime < transitionDuration)
        {
            cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / transitionDuration);
            cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = targetPosition;
        cameraTransform.rotation = targetRotation;

        // Attendre un instant avant de passer à la vue sur le fantôme
        yield return new WaitForSeconds(1f);

        // Passer à la vue sur le fantôme
        Transform cameraFocusPoint = ghost.Find("CameraFocusPoint");
        if (cameraFocusPoint != null)
        {
            Vector3 ghostTargetPosition = cameraFocusPoint.position;
            Vector3 lookAtPoint = ghost.position + Vector3.up * 1.5f; // 1.5f correspond à une hauteur typique au niveau de la tête
            Quaternion ghostTargetRotation = Quaternion.LookRotation(lookAtPoint - ghostTargetPosition);

            elapsedTime = 0f;
            startPosition = cameraTransform.position;
            startRotation = cameraTransform.rotation;

            while (elapsedTime < transitionDuration)
            {
                cameraTransform.position = Vector3.Lerp(startPosition, ghostTargetPosition, elapsedTime / transitionDuration);
                cameraTransform.rotation = Quaternion.Slerp(startRotation, ghostTargetRotation, elapsedTime / transitionDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            cameraTransform.position = ghostTargetPosition;
            cameraTransform.rotation = ghostTargetRotation;

            // Jouer une animation de danse pour le fantôme
            if (ghostAnimator != null)
            {
                int randomIndex = Random.Range(0, ghostDanceAnimations.Length);
                ghostAnimator.SetTrigger(ghostDanceAnimations[randomIndex]);
            }
        }
        else
        {
            Debug.LogWarning("CameraFocusPoint introuvable sur le fantôme.");
        }

        // Afficher le texte "Game Over"
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
    }
}
