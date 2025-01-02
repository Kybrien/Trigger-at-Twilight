using UnityEngine;
using UnityEngine.UI;

public class GhostIndicatorUI : MonoBehaviour
{
    public Transform playerCamera;  // La cam�ra du joueur
    public Image indicatorImage;    // L'image du cercle qui indique la direction
    public Transform ghostTransform; // La position du fant�me � suivre

    public float maxIndicatorSize = 200f;  // Taille maximale du cercle (lorsque loin de la cible)
    public float minIndicatorSize = 50f;   // Taille minimale du cercle (lorsque proche de la cible)

    void Update()
    {
        if (ghostTransform == null)
        {
            // Si aucun fant�me n'est pr�sent, d�sactiver l'image et sortir de la fonction
            indicatorImage.gameObject.SetActive(false);
            return;
        }
        else
        {
            // Si un fant�me est pr�sent, activer l'image
            indicatorImage.gameObject.SetActive(true);
        }

        // Calculer la direction vers le fant�me depuis la cam�ra du joueur
        Vector3 directionToGhost = ghostTransform.position - playerCamera.position;
        directionToGhost.y = 0;  // On ignore la composante Y pour une meilleure indication horizontale

        Vector3 forward = playerCamera.forward;
        forward.y = 0;  // On ignore la composante Y aussi pour le forward

        float angle = Vector3.Angle(forward, directionToGhost);

        // Modifier la taille de l'indicateur en fonction de l'angle
        float t = Mathf.Clamp01(angle / 180f);  // Normaliser l'angle entre 0 et 1
        float indicatorSize = Mathf.Lerp(minIndicatorSize, maxIndicatorSize, t);

        indicatorImage.rectTransform.sizeDelta = new Vector2(indicatorSize, indicatorSize);
    }
}
