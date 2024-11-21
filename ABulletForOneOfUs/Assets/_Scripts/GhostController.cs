using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    public float ghostLifetime = 5f; // Temps avant que le fantôme tue le joueur
    private bool isActive = false;

    [Header("Animator Settings")]
    public Animator animator;
    private DeathCameraController deathCameraController;

    void Start()
    {
        // Le fantôme commence désactivé
        gameObject.SetActive(true);

        deathCameraController = FindObjectOfType<DeathCameraController>();
        if (deathCameraController == null)
        {
            Debug.LogWarning("DeathCameraController est introuvable dans la scène.");
        }
    }

    public void Spawn(Vector3 spawnPosition)
    {
        // Assurer que le fantôme est actif
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        isActive = true;

        // Lancer le timer pour que le fantôme tue le joueur s'il n'est pas détruit
        Invoke("GhostAttack", ghostLifetime);
    }

    void Update()
    {
        if (isActive)
        {
            // Faire face au joueur
            Transform playerTransform = Camera.main.transform;
            Vector3 lookDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            transform.LookAt(lookDirection);
        }
    }

    void GhostAttack()
    {
        if (isActive)
        {
            Debug.Log("Le fantôme a tué le joueur !");

            if (deathCameraController != null)
            {
                deathCameraController.TriggerDeathSequence();
            }
        }
    }

    public void DestroyGhost()
    {
        if (animator != null)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(DestroyAfterAnimation()); // Attendre la fin de l'animation de mort avant de détruire
        }
        else
        {
            Destroy(gameObject);
        }

        isActive = false;

        GhostIndicatorUI ghostIndicator = FindObjectOfType<GhostIndicatorUI>();
        if (ghostIndicator != null)
        {
            ghostIndicator.ghostTransform = null;
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        if (animator != null)
        {
            // Attendre que l'animation courante soit terminée
            AnimatorStateInfo currentState;
            do
            {
                yield return null;
                currentState = animator.GetCurrentAnimatorStateInfo(0);
            } while (currentState.normalizedTime < 2.0f);
        }

        // Détruire le fantôme après la fin de l'animation
        Destroy(gameObject);
    }
}
