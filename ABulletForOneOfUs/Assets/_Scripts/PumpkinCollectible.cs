using UnityEngine;
using UnityEngine.UI;

public class PumpkinCollectible : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionTime = 2f; // Temps n�cessaire pour interagir
    public KeyCode interactionKey = KeyCode.E; // Touche pour interagir
    public LayerMask interactionLayer; // Couches � consid�rer pour le raycast

    [Header("Visual Feedback")]
    public Image progressIndicator; // R�f�rence � l'image de progression

    private float interactionProgress = 0f; // Temps �coul� pendant l'interaction
    private bool isLookingAt = false; // Indique si le joueur regarde la citrouille
    private Camera activeCamera; // R�f�rence � la cam�ra active

    void Start()
    {
        if (progressIndicator != null)
        {
            progressIndicator.gameObject.SetActive(false); // D�sactive l'indicateur au d�marrage
        }
    }

    void Update()
    {
        // V�rifier dynamiquement si la cam�ra active est disponible
        if (activeCamera == null)
        {
            activeCamera = Camera.main;
            if (activeCamera == null) return; // Si aucune cam�ra n'est active, sortir de la m�thode
        }

        // V�rifier si le joueur regarde la citrouille
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, interactionLayer))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isLookingAt = true;

                // D�tecter l�interaction
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
