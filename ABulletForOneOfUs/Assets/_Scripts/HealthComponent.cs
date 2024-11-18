using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died!");
        // Pour l'instant, on détruit simplement l'objet, mais vous pouvez ajouter une logique supplémentaire ici
        Destroy(gameObject);
    }
}
