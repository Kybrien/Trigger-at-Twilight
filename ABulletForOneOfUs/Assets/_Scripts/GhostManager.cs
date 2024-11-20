using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject ghostPrefab;  // Le préfab du fantôme
    public Transform playerTransform;  // Référence au joueur (pour déterminer sa position)
    public float spawnRadius = 30f;  // Rayon dans lequel le fantôme peut apparaître autour du joueur
    public string spawnableTag = "Spawnable";  // Tag des surfaces où le fantôme peut apparaître
    public GhostIndicatorUI ghostIndicatorUI; // Référence au script d'UI d'indicateur

    void Update()
    {
        // Pour tester l'apparition avec une touche (par exemple, la touche G)
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnGhost();
        }
    }

    void SpawnGhost()
    {
        if (playerTransform != null)
        {
            for (int i = 0; i < 10; i++) // Essayer plusieurs fois pour trouver une position valide
            {
                // Générer une position aléatoire autour du joueur dans un rayon donné
                Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;

                // Garder la hauteur aléatoire
                Vector3 spawnPosition = playerTransform.position + randomDirection;

                // Vérifier avec un Raycast depuis au-dessus de la position si c'est une surface appropriée
                if (Physics.Raycast(spawnPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag(spawnableTag))
                    {
                        // La surface a le bon tag, on peut faire apparaître le fantôme
                        Vector3 finalSpawnPosition = hit.point;
                        GameObject ghostInstance = Instantiate(ghostPrefab, finalSpawnPosition, Quaternion.identity);

                        // Mettre à jour la référence dans GhostIndicatorUI
                        if (ghostIndicatorUI != null)
                        {
                            ghostIndicatorUI.ghostTransform = ghostInstance.transform;
                        }

                        // Lancer le comportement du fantôme
                        GhostController ghostController = ghostInstance.GetComponent<GhostController>();
                        if (ghostController != null)
                        {
                            ghostController.Spawn(finalSpawnPosition);
                        }

                        return; // Terminer la fonction une fois le fantôme spawné
                    }
                }
            }

            Debug.LogWarning("Impossible de trouver une surface appropriée pour faire apparaître le fantôme après plusieurs tentatives.");
        }
    }
}
