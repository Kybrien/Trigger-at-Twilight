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

        // D�truire la balle apr�s l'impact
        Destroy(gameObject);
    }
}
