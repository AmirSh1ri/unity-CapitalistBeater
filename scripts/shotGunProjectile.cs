using UnityEngine;

public class shotGunProjectile : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public ParticleSystem muzzleFlash;
    public float bulletSpeed = 20f;

    void Update()
    {
    }

    public void ShootBullet()
    {
        muzzleFlash?.Play();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
    }
}
