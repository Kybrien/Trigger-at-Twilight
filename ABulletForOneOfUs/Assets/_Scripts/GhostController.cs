using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    public float ghostLifetime = 5f; // Temps avant que le fantôme tue le joueur
    public float slowMotionScale = 0.2f; // Facteur de ralentissement du temps
    private bool isActive = false;

    void Start()
    {
        // Le fantôme commence désactivé
        gameObject.SetActive(true);
    }

    public void Spawn(Vector3 spawnPosition)
    {
        // Assurer que le fantôme est actif
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        isActive = true;

        // Ralentir le temps
        Time.timeScale = slowMotionScale;

        // Lancer le timer pour que le fantôme tue le joueur s'il n'est pas détruit
        Invoke("GhostAttack", ghostLifetime);
    }

    void Update()
    {
        if (isActive)
        {
            // Faire face au joueur (optionnel selon le comportement souhaité)
            Transform playerTransform = Camera.main.transform;
            transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        }
    }

    void GhostAttack()
    {
        if (isActive)
        {
            Debug.Log("Le fantôme a tué le joueur !");
            // Ici, implémentez la logique pour terminer la partie ou infliger des dégâts mortels
            // Exemple : FindObjectOfType<HealthComponent>().TakeDamage(999);

            // Remettre le temps à la normale après l'attaque
            Time.timeScale = 1f;

            // Détruire le fantôme
            Destroy(gameObject);
        }
    }

    public void DestroyGhost()
    {
        // Remettre le temps à la normale si le joueur détruit le fantôme
        Time.timeScale = 1f;
        isActive = false;
        GhostIndicatorUI ghostIndicator = FindObjectOfType<GhostIndicatorUI>();
        if (ghostIndicator != null)
        {
            ghostIndicator.ghostTransform = null;
        }
        Destroy(gameObject);
    }
}
