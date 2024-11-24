using UnityEngine;
using UnityEngine.UI;

public class PumpkinCollectible : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionTime = 2f; // Temps n�cessaire pour interagir
    public KeyCode interactionKey = KeyCode.E; // Touche pour interagir

    [Header("Visual Feedback")]
    public Image progressIndicator; // R�f�rence � l'image de progression

    private float interactionProgress = 0f; // Temps �coul� pendant l'interaction
    private bool isInteracting = false; // Indique si l'interaction est en cours

    void Update()
    {
        // V�rifier si le joueur interagit avec la citrouille
        if (isInteracting)
        {
            if (Input.GetKey(interactionKey))
            {
                interactionProgress += Time.deltaTime;

                // Mettre � jour l'indicateur visuel
                if (progressIndicator != null)
                {
                    progressIndicator.fillAmount = interactionProgress / interactionTime;
                }

                // V�rifier si l'interaction est termin�e
                if (interactionProgress >= interactionTime)
                {
                    Debug.Log("Citrouille collectee");
                    Collect();
                }
            }
            else
            {
                ResetInteraction();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // V�rifie si le joueur entre dans la zone d'interaction
        if (other.CompareTag("Player"))
        {
            isInteracting = true;

            // Activer l'indicateur visuel
            if (progressIndicator != null)
            {
                progressIndicator.gameObject.SetActive(true);
                progressIndicator.fillAmount = 0f; // R�initialiser le remplissage
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // V�rifie si le joueur sort de la zone d'interaction
        if (other.CompareTag("Player"))
        {
            ResetInteraction();
        }
    }

    private void ResetInteraction()
    {
        isInteracting = false;
        interactionProgress = 0f;

        // D�sactiver l'indicateur visuel
        if (progressIndicator != null)
        {
            progressIndicator.gameObject.SetActive(false);
        }
    }

    private void Collect()
    {
        // Notifie le CollectibleManager que la citrouille a �t� collect�e
        CollectibleManager.Instance.CollectPumpkin();

        // D�truire la citrouille
        Destroy(gameObject);
    }
}
