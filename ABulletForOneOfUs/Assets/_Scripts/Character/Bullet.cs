using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

    void OnCollisionEnter(Collision collision)
    {
        HealthComponent target = collision.gameObject.GetComponent<HealthComponent>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // Effet d'impact
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(Resources.Load("ImpactEffect"), contact.point, Quaternion.LookRotation(contact.normal));
        }

        // Détruire la balle après l'impact
        Destroy(gameObject);
    }
}
