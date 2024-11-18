using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public Transform firePoint;  // Position d'o� la balle est tir�e
    public GameObject bulletPrefab;  // Pr�fab du projectile (la balle)
    public float bulletSpeed = 100f;  // Vitesse de la balle
    public float maxRange = 100f;  // Port�e maximale du tir
    public int magazineSize = 3;  // Capacit� du chargeur
    public float shootCooldown = 0.5f;  // Temps entre chaque tir
    public LayerMask hitLayer;  // Couches que le tir peut toucher
    public GameObject shootFXPrefab;  // Pr�fab de l'effet visuel pour le tir
    public Animator animator;

    [Header("Raycast Settings")]
    public ParticleSystem muzzleFlash;  // Effet de flash du canon
    public GameObject impactEffect;  // Effet d'impact

    [Header("Sound Settings")]
    public AudioClip shootSound;  // Son du tir
    public AudioClip reloadSound;  // Son du rechargement
    private AudioSource audioSource;

    private bool canShoot = true;
    private bool isReloading = false;
    private int currentAmmo;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentAmmo = magazineSize;  // Charger le chargeur au d�but
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            canShoot = false;
        }

        // Tir
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }

        // Rechargement
        if (Input.GetButtonDown("Reload") && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        // Emp�che de tirer jusqu'� la fin du cooldown
        canShoot = false;
        Invoke("ResetShoot", shootCooldown);

        // R�duire le nombre de munitions
        currentAmmo--;

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

        // Cr�er l'effet FX au point de tir
        if (shootFXPrefab != null)
        {
            Instantiate(shootFXPrefab, firePoint.position, firePoint.rotation);
        }

        // Utiliser la cam�ra principale pour tirer le raycast
        Camera playerCamera = Camera.main;
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxRange, hitLayer))
        {
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

        // Cr�er une balle physique qui se d�place pour plus de visuel (suivant la direction de la cam�ra)
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = rayDirection * bulletSpeed;
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        canShoot = false;

        // Jouer l'animation de rechargement
        if (animator != null)
        {
            animator.SetTrigger("Reloading");
        }

        // Jouer le son de rechargement
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        // Attendre le temps de rechargement (2 secondes par exemple)
        yield return new WaitForSeconds(2f);

        currentAmmo = magazineSize;  // Remplir le chargeur
        canShoot = true;
        isReloading = false;
    }

    void ResetShoot()
    {
        if (currentAmmo > 0)
        {
            canShoot = true;
        }
    }
}
