using UnityEngine;
using UnityEngine.UI;

public class PumpkinCollectible : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionTime = 2f; // Temps nécessaire pour interagir
    public KeyCode interactionKey = KeyCode.E; // Touche pour interagir

    [Header("Visual Feedback")]
    public Image progressIndicator; // Référence à l'image de progression

    private float interactionProgress = 0f; // Temps écoulé pendant l'interaction
    private bool isInteracting = false; // Indique si l'interaction est en cours

    void Update()
    {
        // Vérifier si le joueur interagit avec la citrouille
        if (isInteracting)
        {
            if (Input.GetKey(interactionKey))
            {
                interactionProgress += Time.deltaTime;

                // Mettre à jour l'indicateur visuel
                if (progressIndicator != null)
                {
                    progressIndicator.fillAmount = interactionProgress / interactionTime;
                }

                // Vérifier si l'interaction est terminée
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
        // Vérifie si le joueur entre dans la zone d'interaction
        if (other.CompareTag("Player"))
        {
            isInteracting = true;

            // Activer l'indicateur visuel
            if (progressIndicator != null)
            {
                progressIndicator.gameObject.SetActive(true);
                progressIndicator.fillAmount = 0f; // Réinitialiser le remplissage
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Vérifie si le joueur sort de la zone d'interaction
        if (other.CompareTag("Player"))
        {
            ResetInteraction();
        }
    }

    private void ResetInteraction()
    {
        isInteracting = false;
        interactionProgress = 0f;

        // Désactiver l'indicateur visuel
        if (progressIndicator != null)
        {
            progressIndicator.gameObject.SetActive(false);
        }
    }

    private void Collect()
    {
        // Notifie le CollectibleManager que la citrouille a été collectée
        CollectibleManager.Instance.CollectPumpkin();

        // Détruire la citrouille
        Destroy(gameObject);
    }
}
