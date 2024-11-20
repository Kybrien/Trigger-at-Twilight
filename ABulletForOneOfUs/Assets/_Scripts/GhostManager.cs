using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject ghostPrefab;  // Le pr�fab du fant�me
    public Transform playerTransform;  // R�f�rence au joueur (pour d�terminer sa position)
    public float spawnRadius = 30f;  // Rayon dans lequel le fant�me peut appara�tre autour du joueur
    public string spawnableTag = "Spawnable";  // Tag des surfaces o� le fant�me peut appara�tre
    public GhostIndicatorUI ghostIndicatorUI; // R�f�rence au script d'UI d'indicateur

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
                // G�n�rer une position al�atoire autour du joueur dans un rayon donn�
                Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;

                // Garder la hauteur al�atoire
                Vector3 spawnPosition = playerTransform.position + randomDirection;

                // V�rifier avec un Raycast depuis au-dessus de la position si c'est une surface appropri�e
                if (Physics.Raycast(spawnPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag(spawnableTag))
                    {
                        // La surface a le bon tag, on peut faire appara�tre le fant�me
                        Vector3 finalSpawnPosition = hit.point;
                        GameObject ghostInstance = Instantiate(ghostPrefab, finalSpawnPosition, Quaternion.identity);

                        // Mettre � jour la r�f�rence dans GhostIndicatorUI
                        if (ghostIndicatorUI != null)
                        {
                            ghostIndicatorUI.ghostTransform = ghostInstance.transform;
                        }

                        // Lancer le comportement du fant�me
                        GhostController ghostController = ghostInstance.GetComponent<GhostController>();
                        if (ghostController != null)
                        {
                            ghostController.Spawn(finalSpawnPosition);
                        }

                        return; // Terminer la fonction une fois le fant�me spawn�
                    }
                }
            }

            Debug.LogWarning("Impossible de trouver une surface appropri�e pour faire appara�tre le fant�me apr�s plusieurs tentatives.");
        }
    }
}
