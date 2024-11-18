using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public Transform firePoint;  // Position d'o� la balle est tir�e
    public GameObject bulletPrefab;  // Pr�fab du projectile (la balle)
    public float bulletSpeed = 100f;  // Vitesse de la balle
    public float maxRange = 100f;  // Port�e maximale du tir
    public LayerMask hitLayer;  // Couches que le tir peut toucher

    [Header("Raycast Settings")]
    public ParticleSystem muzzleFlash;  // Effet de flash du canon
    public GameObject impactEffect;  // Effet d'impact


    [Header("Sound Settings")]
    public AudioClip shootSound;  // Son du tir
    public AudioSource audioSource;

    private bool canShoot = true;
    private float shootCooldown = 0.3f;  // Temps entre chaque tir (1 sec pour une balle)

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Emp�che de tirer jusqu'� la fin du cooldown
        canShoot = false;
        Invoke("ResetShoot", shootCooldown);

        // Jouer le son du tir
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Effet de flash du canon
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Utiliser la direction de la cam�ra pour le Raycast
        Camera playerCamera = Camera.main; // Utiliser la cam�ra principale
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        // Lancer le raycast
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxRange, hitLayer))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // Cr�er l'effet d'impact si on touche quelque chose
            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            // V�rifier si l'objet touch� a un "HealthComponent"
            HealthComponent target = hit.collider.GetComponent<HealthComponent>();
            if (target != null)
            {
                target.TakeDamage(100);  // Inflige des d�g�ts (100 pour un kill direct)
            }
        }

        // Cr�er une balle physique qui se d�place pour plus de visuel
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * bulletSpeed;
            }
        }

        // Afficher le raycast dans la sc�ne pour aider � d�boguer le d�calage
        Debug.DrawRay(rayOrigin, rayDirection * maxRange, Color.red, 100.0f);


        // Dessiner une ligne en jeu pour visualiser le tir
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, firePoint.position + firePoint.forward * maxRange);

        // D�truire la ligne apr�s une courte dur�e
        Destroy(lineRenderer, 0.10f);
    }


    void ResetShoot()
    {
        canShoot = true;
    }
}
