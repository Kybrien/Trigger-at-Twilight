using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject ghostPrefab;  // Le pr�fab du fant�me
    public Transform playerTransform;  // R�f�rence au joueur
    public float spawnRadius = 30f;  // Rayon pour le spawn autour du joueur
    public string spawnableTag = "Spawnable";  // Tag des surfaces o� le fant�me peut appara�tre
    public GhostIndicatorUI ghostIndicatorUI; // R�f�rence au script d'UI d'indicateur

    public float initialSpawnDelay = 4f;  // D�lai avant le premier spawn
    public Vector2 respawnDelayRange = new Vector2(4f, 6f);  // Plage de temps pour le respawn

    private bool isGhostActive = false;

    public void InitializeGhostSpawning()
    {
        // Lancer le premier spawn apr�s le d�lai initial
        Invoke(nameof(SpawnGhost), initialSpawnDelay);
    }

    public void SpawnGhost()
    {
        if (playerTransform != null)
        {
            for (int i = 0; i < 20; i++) // Essayer plusieurs fois pour trouver une position valide
            {
                Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
                Vector3 spawnPosition = playerTransform.position + randomDirection;

                if (Physics.Raycast(spawnPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag(spawnableTag))
                    {
                        Vector3 finalSpawnPosition = hit.point;
                        GameObject ghostInstance = Instantiate(ghostPrefab, finalSpawnPosition, Quaternion.identity);

                        if (ghostIndicatorUI != null)
                        {
                            ghostIndicatorUI.ghostTransform = ghostInstance.transform;
                        }

                        GhostController ghostController = ghostInstance.GetComponent<GhostController>();
                        if (ghostController != null)
                        {
                            ghostController.Spawn(finalSpawnPosition);
                            ghostController.OnGhostDestroyed += HandleGhostDestroyed; // �couter l'�v�nement
                        }

                        return;
                    }
                }
            }

            Debug.LogWarning("Impossible de trouver une surface appropri�e pour faire appara�tre le fant�me apr�s plusieurs tentatives.");
        }
    }

    void HandleGhostDestroyed()
    {
        isGhostActive = false;

        // Planifie un nouveau spawn avec un d�lai al�atoire
        float randomRespawnDelay = Random.Range(respawnDelayRange.x, respawnDelayRange.y);
        Invoke(nameof(SpawnGhost), randomRespawnDelay);
    }
}
