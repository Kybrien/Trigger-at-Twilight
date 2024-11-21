using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //[SerializeField] private Vector3 movementDirection = new Vector3(1f, 0f, 0f); // Direction du mouvement
    [SerializeField] private float movementSpeed = 2f; // Vitesse de déplacement
    [SerializeField] private float movementRange = 3f; // Distance maximale parcourue

    private Vector3 startPosition;
    private bool movingForward = true;

    private void Start()
    {
        startPosition = transform.position;
    }

    /*private void Update()
    {
        MovePlatform();
    }*/

    /*private void MovePlatform()
    {
        // Calcul de la nouvelle position
        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance >= movementRange)
        {
            movingForward = !movingForward; // Inverse la direction
        }

        Vector3 direction = movingForward ? movementDirection : -movementDirection;
        transform.Translate(direction * movementSpeed * Time.deltaTime);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        // Attache le joueur lorsque qu'il entre en contact avec la plateforme
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Libère le joueur lorsqu'il quitte la plateforme
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}