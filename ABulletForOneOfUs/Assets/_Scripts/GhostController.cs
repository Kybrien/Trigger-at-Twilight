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

    public delegate void GhostDestroyedHandler();
    public event GhostDestroyedHandler OnGhostDestroyed;

    void Start()
    {
        gameObject.SetActive(true);

        deathCameraController = FindObjectOfType<DeathCameraController>();
        if (deathCameraController == null)
        {
            Debug.LogWarning("DeathCameraController est introuvable dans la scène.");
        }
    }

    public void Spawn(Vector3 spawnPosition)
    {
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        isActive = true;

        // Lancer le timer pour tuer le joueur
        Invoke(nameof(GhostAttack), ghostLifetime);
    }

    void Update()
    {
        if (isActive)
        {
            if (Camera.main != null)
            {
                Transform playerTransform = Camera.main.transform;
                Vector3 lookDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
                transform.LookAt(lookDirection);
            }
        }
    }

    void GhostAttack()
    {
        if (isActive)
        {
            Debug.Log("Le fantôme a tué le joueur !");
            deathCameraController?.TriggerDeathSequence();
        }
    }

    public void DestroyGhost()
    {
        if (animator != null)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            Destroy(gameObject);
        }

        // Notifier les abonnés que le fantôme a été détruit
        OnGhostDestroyed?.Invoke();
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
            AnimatorStateInfo currentState;
            do
            {
                yield return null;
                currentState = animator.GetCurrentAnimatorStateInfo(0);
            } while (currentState.normalizedTime < 1.0f);
        }

        Destroy(gameObject);
    }
}
