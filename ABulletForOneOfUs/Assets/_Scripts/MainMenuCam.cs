using System.Collections;
using UnityEngine;

public class MainMenuCam : MonoBehaviour
{
    [Header("Camera Transition Settings")]
    public Transform playerTransform; // La référence au joueur
    public Transform menuPosition; // La position initiale de la caméra pour le menu principal
    public GameObject mainMenuUI; // UI du menu principal à désactiver après avoir cliqué sur "Play"
    public float transitionDuration = 3f; // Durée de la transition vers le joueur
    public GameObject phantomBackground;
    public Camera mainMenuCamera; // Caméra du menu principal
    public GameObject player; // GameObject du joueur

    private bool transitioning = false;

    void Start()
    {
        // Assurer que la caméra commence à la position du menu
        transform.position = menuPosition.position;
        transform.rotation = menuPosition.rotation;

        // Assurer que le joueur est désactivé au début
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
        }
    }

    private IEnumerator TransitionToPlayer()
    {
        transitioning = true;

        // Désactiver le menu principal
        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(false);
        }

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Vector3 endPosition = playerTransform.position + new Vector3(0, 2, -4); // Ajuster l'offset pour être derrière le joueur
        Quaternion endRotation = Quaternion.LookRotation(playerTransform.position - transform.position);

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionDuration);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assurer que la caméra soit exactement à la position finale
        transform.position = endPosition;
        transform.rotation = endRotation;

        // Désactiver la caméra du menu principal et activer le joueur
        if (mainMenuCamera != null)
        {
            mainMenuCamera.gameObject.SetActive(false);
        }
        if (player != null)
        {
            player.SetActive(true);
        }

        transitioning = false;
    }
}
