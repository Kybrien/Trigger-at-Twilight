using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    public float ghostLifetime = 5f; // Temps avant que le fant�me tue le joueur
    private bool isActive = false;

    [Header("Animator Settings")]
    public Animator animator;

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

        // Lancer le timer pour que le fant�me tue le joueur s'il n'est pas d�truit
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
            Debug.Log("Le fant�me a tu� le joueur !");

            // Lancer une animation al�atoire avant de d�truire le fant�me
            if (animator != null)
            {
                int rand = Random.Range(0, 4);
                switch (rand)
                {
                    case 0:
                        animator.SetTrigger("Shuffling");
                        break;
                    case 1:
                        animator.SetTrigger("Hiphop");
                        break;
                    case 2:
                        animator.SetTrigger("Twerk");
                        break;
                    default:
                        animator.SetTrigger("BreakDance");
                        break;
                }

                // Attendre la fin de l'animation avant de d�truire le fant�me
                StartCoroutine(DestroyAfterAnimation());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void DestroyGhost()
    {
        if (animator != null)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(DestroyAfterAnimation()); // Attendre la fin de l'animation de mort avant de d�truire
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
            // Attendre que l'animation courante soit termin�e
            AnimatorStateInfo currentState;
            do
            {
                yield return null;
                currentState = animator.GetCurrentAnimatorStateInfo(0);
            } while (currentState.normalizedTime < 1.0f);
        }

        // D�truire le fant�me apr�s la fin de l'animation
        Destroy(gameObject);
    }
}
