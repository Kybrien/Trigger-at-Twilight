using UnityEngine;
using UnityEngine.UI;

public class GhostIndicatorUI : MonoBehaviour
{
    public Transform playerCamera;  // La caméra du joueur
    public Image indicatorImage;    // L'image du cercle qui indique la direction
    public Transform ghostTransform; // La position du fantôme à suivre

    public float maxIndicatorSize = 200f;  // Taille maximale du cercle (lorsque loin de la cible)
    public float minIndicatorSize = 50f;   // Taille minimale du cercle (lorsque proche de la cible)

    void Update()
    {
        if (ghostTransform == null || playerCamera == null || indicatorImage == null)
        {
            indicatorImage.gameObject.SetActive(false); // Désactiver si pas de fantôme à suivre
            return;
        }

        // Calculer la direction vers le fantôme depuis la caméra du joueur
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
