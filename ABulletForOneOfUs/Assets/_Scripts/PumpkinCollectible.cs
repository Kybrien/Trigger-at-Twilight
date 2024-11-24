using UnityEngine;
using UnityEngine.UI;

public class PumpkinCollectible : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionTime = 2f; // Temps nécessaire pour interagir
    public KeyCode interactionKey = KeyCode.E; // Touche pour interagir
    public LayerMask interactionLayer; // Couches à considérer pour le raycast

    [Header("Visual Feedback")]
    public Image progressIndicator; // Référence à l'image de progression

    private float interactionProgress = 0f; // Temps écoulé pendant l'interaction
    private bool isLookingAt = false; // Indique si le joueur regarde la citrouille
    private Camera activeCamera; // Référence à la caméra active

    void Start()
    {
        if (progressIndicator != null)
        {
            progressIndicator.gameObject.SetActive(false); // Désactive l'indicateur au démarrage
        }
    }

    void Update()
    {
        // Vérifier dynamiquement si la caméra active est disponible
        if (activeCamera == null)
        {
            activeCamera = Camera.main;
            if (activeCamera == null) return; // Si aucune caméra n'est active, sortir de la méthode
        }

        // Vérifier si le joueur regarde la citrouille
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, interactionLayer))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isLookingAt = true;

                // Détecter l’interaction
                if (Input.GetKey(interactionKey))
                {
                    interactionProgress += Time.deltaTime;

                    if (progressIndicator != null)
                    {
                        progressIndicator.fillAmount = interactionProgress / interactionTime;
                        progressIndicator.gameObject.SetActive(true);
                    }

                    if (interactionProgress >= interactionTime)
                    {
                        Collect();
                    }
                }
                else
                {
                    ResetInteraction();
                }
            }
            else
            {
                ResetInteraction();
            }
        }
        else
        {
            ResetInteraction();
        }
    }

    private void ResetInteraction()
    {
        isLookingAt = false;
        interactionProgress = 0f;

        if (progressIndicator != null)
        {
            progressIndicator.fillAmount = 0f;
            progressIndicator.gameObject.SetActive(false);
        }
    }

    private void Collect()
    {
        Debug.Log("Pumpkin collected!");

        CollectibleManager.Instance.CollectPumpkin();

        Destroy(gameObject);
    }
}
