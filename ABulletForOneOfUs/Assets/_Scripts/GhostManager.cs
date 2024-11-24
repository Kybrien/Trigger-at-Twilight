using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject ghostPrefab; // Le prefab du fant�me
    public Transform playerTransform; // R�f�rence au joueur
    public float spawnRadius = 30f; // Rayon pour le spawn autour du joueur
    public string spawnableTag = "Spawnable"; // Tag des surfaces o� le fant�me peut appara�tre
    public GhostIndicatorUI ghostIndicatorUI; // R�f�rence au script d'UI d'indicateur

    [Header("Spawn Sounds")]
    public AudioClip[] spawnSounds; // Tableau de sons � jouer au spawn
    public AudioSource audioSource; // Source audio pour jouer les sons

    public float initialSpawnDelay = 4f; // D�lai avant le premier spawn
    public float respawnDelay = 3f; // D�lai entre les spawns

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

                        // Mettre � jour la r�f�rence dans GhostIndicatorUI
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

                        // Jouer un son al�atoire au spawn
                        PlaySpawnSound();

                        return;
                    }
                }
            }

            Debug.LogWarning("Impossible de trouver une surface appropri�e pour faire appara�tre le fant�me apr�s plusieurs tentatives.");
        }
    }

    private void PlaySpawnSound()
    {
        if (spawnSounds.Length > 0 && audioSource != null)
        {
            // Choisir un son al�atoire
            AudioClip randomClip = spawnSounds[Random.Range(0, spawnSounds.Length)];
            audioSource.PlayOneShot(randomClip);
        }
    }

    void HandleGhostDestroyed()
    {
        isGhostActive = false;
        // Relancer le spawn apr�s un d�lai
        Invoke(nameof(SpawnGhost), respawnDelay);
    }
}
