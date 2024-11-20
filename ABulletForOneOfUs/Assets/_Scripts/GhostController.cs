using UnityEngine;
using System.Collections;
public class GhostController : MonoBehaviour
{
    [Header("Ghost Settings")]
    public float ghostLifetime = 5f; // Temps avant que le fant�me tue le joueur
    public float slowMotionScale = 0.2f; // Facteur de ralentissement du temps
    private bool isActive = false;

    private float originalFixedDeltaTime;

    [Header("Animator Settings")]
    public Animator animator;

    void Start()
    {
        // Le fant�me commence d�sactiv�
        gameObject.SetActive(true);

        // Sauvegarder la valeur originale de fixedDeltaTime pour pouvoir la restaurer plus tard
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Spawn(Vector3 spawnPosition)
    {
        // Assurer que le fant�me est actif
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        isActive = true;

        // Ralentir le temps
        Time.timeScale = slowMotionScale;

        // Ajuster Time.fixedDeltaTime proportionnellement pour conserver la fluidit�
        Time.fixedDeltaTime = originalFixedDeltaTime * slowMotionScale;

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
            // Remettre le temps � la normale apr�s l'attaque
            ResetTimeScale();

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

        // Remettre le temps � la normale si le joueur d�truit le fant�me
        ResetTimeScale();
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
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            // Attendre que l'animation courante soit termin�e
            while (currentState.normalizedTime < 1.0f && !currentState.IsTag("Dead"))
            {
                yield return null;
                currentState = animator.GetCurrentAnimatorStateInfo(0);
            }
        }

        // D�truire le fant�me apr�s la fin de l'animation
        Destroy(gameObject);
    }

    void ResetTimeScale()
    {
        // Restaurer Time.fixedDeltaTime avant de restaurer Time.timeScale
        Time.fixedDeltaTime = originalFixedDeltaTime;
        Time.timeScale = 1f;
    }
}
