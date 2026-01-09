using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    public int attackDamage = 40;
    public Attack attackCode;

    private void Start()
    {
        if (attackCode == null)
        {
            attackCode = FindObjectOfType<Attack>();
        }

        Destroy(gameObject, 0.15f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
                if (attackCode != null && !attackCode.isUltimateActive)
                    attackCode.ultIncrease();
            }
            else
            {
                Debug.Log("null");
            }
        }

        Destroy(gameObject);
    }
}
