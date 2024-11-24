using System.Collections;
using UnityEngine;

public class MainMenuCam : MonoBehaviour
{
    [Header("Camera Transition Settings")]
    public Transform playerTransform; // La r�f�rence au joueur
    public Transform menuPosition; // La position initiale de la cam�ra pour le menu principal
    public GameObject mainMenuUI; // UI du menu principal
    public float transitionDuration = 3f; // Dur�e de la transition vers le joueur
    public GameObject phantomBackground;
    public Camera mainMenuCamera; // Cam�ra du menu principal
    public GameObject player; // GameObject du joueur
    public TimerController timerController;
    public GhostManager ghostManager;

    private bool transitioning = false;

    void Start()
    {
        // Positionner la cam�ra pour le menu principal
        transform.position = menuPosition.position;
        transform.rotation = menuPosition.rotation;

        // D�sactiver le joueur au d�marrage
        if (player != null)
        {
            player.SetActive(false);
        }
    }

    public void OnPlayButtonClicked()
    {
        if (!transitioning)
        {
            StartCoroutine(TransitionToPlayer());

            if (phantomBackground != null)
            {
                phantomBackground.SetActive(false);
            }

            // Lancer le spawn des fant�mes
            if (ghostManager != null)
            {
                ghostManager.InitializeGhostSpawning();
            }
            else
            {
                Debug.LogError("GhostManager non assign� !");
            }
        }
    }

    private IEnumerator TransitionToPlayer()
    {
        transitioning = true;

        // D�sactiver le menu principal
        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(false);
        }

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Vector3 endPosition = playerTransform.position + new Vector3(0, 2, -4);
        Quaternion endRotation = Quaternion.LookRotation(playerTransform.position - transform.position);

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionDuration);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        transform.rotation = endRotation;

        // Activer le joueur
        if (mainMenuCamera != null)
        {
            mainMenuCamera.gameObject.SetActive(false);
        }
        if (player != null)
        {
            player.SetActive(true);
        }

        // Reset du timer
        if (timerController != null)
        {
            timerController.ResetTimer();
        }

        transitioning = false;
    }
}
