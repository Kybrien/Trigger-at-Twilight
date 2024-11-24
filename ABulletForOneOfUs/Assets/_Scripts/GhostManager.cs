using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject ghostPrefab; // Le prefab du fantôme
    public Transform playerTransform; // Référence au joueur
    public float spawnRadius = 30f; // Rayon pour le spawn autour du joueur
    public string spawnableTag = "Spawnable"; // Tag des surfaces où le fantôme peut apparaître
    public GhostIndicatorUI ghostIndicatorUI; // Référence au script d'UI d'indicateur

    [Header("Spawn Sounds")]
    public AudioClip[] spawnSounds; // Tableau de sons à jouer au spawn
    public AudioSource audioSource; // Source audio pour jouer les sons

    public float initialSpawnDelay = 4f; // Délai avant le premier spawn
    public float respawnDelay = 3f; // Délai entre les spawns

    private bool isGhostActive = false;

    public void InitializeGhostSpawning()
    {
        // Lancer le premier spawn après le délai initial
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

                        // Mettre à jour la référence dans GhostIndicatorUI
                        if (ghostIndicatorUI != null)
                        {
                            ghostIndicatorUI.ghostTransform = ghostInstance.transform;
                        }

                        GhostController ghostController = ghostInstance.GetComponent<GhostController>();
                        if (ghostController != null)
                        {
                            ghostController.Spawn(finalSpawnPosition);
                            ghostController.OnGhostDestroyed += HandleGhostDestroyed;
                        }

                        // Jouer un son aléatoire au spawn
                        PlaySpawnSound();

                        return;
                    }
                }
            }

            Debug.LogWarning("Impossible de trouver une surface appropriée pour faire apparaître le fantôme après plusieurs tentatives.");
        }
    }

    private void PlaySpawnSound()
    {
        if (spawnSounds.Length > 0 && audioSource != null)
        {
            // Choisir un son aléatoire
            AudioClip randomClip = spawnSounds[Random.Range(0, spawnSounds.Length)];
            audioSource.PlayOneShot(randomClip);
        }
    }

    void HandleGhostDestroyed()
    {
        isGhostActive = false;
        // Relancer le spawn après un délai
        Invoke(nameof(SpawnGhost), respawnDelay);
    }
}
