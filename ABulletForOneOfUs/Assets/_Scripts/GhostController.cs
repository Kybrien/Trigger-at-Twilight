using UnityEngine;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    public float ghostLifetime = 5f; // Temps avant que le fant�me tue le joueur
    public float slowMotionScale = 0.2f; // Facteur de ralentissement du temps
    private bool isActive = false;

    void Start()
    {
        // Le fant�me commence d�sactiv�
        gameObject.SetActive(true);
    }

    public void Spawn(Vector3 spawnPosition)
    {
        // Assurer que le fant�me est actif
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        isActive = true;

        // Ralentir le temps
        Time.timeScale = slowMotionScale;

        // Lancer le timer pour que le fant�me tue le joueur s'il n'est pas d�truit
        Invoke("GhostAttack", ghostLifetime);
    }

    void Update()
    {
        if (isActive)
        {
            // Faire face au joueur (optionnel selon le comportement souhait�)
            Transform playerTransform = Camera.main.transform;
            transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        }
    }

    void GhostAttack()
    {
        if (isActive)
        {
            Debug.Log("Le fant�me a tu� le joueur !");
            // Ici, impl�mentez la logique pour terminer la partie ou infliger des d�g�ts mortels
            // Exemple : FindObjectOfType<HealthComponent>().TakeDamage(999);

            // Remettre le temps � la normale apr�s l'attaque
            Time.timeScale = 1f;

            // D�truire le fant�me
            Destroy(gameObject);
        }
    }

    public void DestroyGhost()
    {
        // Remettre le temps � la normale si le joueur d�truit le fant�me
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
