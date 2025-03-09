using System.Linq;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject aoeAttackObject; // Add a public field for the AOE Attack GameObject

    void Start()
    {
        // Destroy the projectile after 5 seconds
        Destroy(gameObject, 10f);

        // Ignore collision with objects that have the tag "Enemy"
        Collider[] enemyColliders = GameObject.FindGameObjectsWithTag("Enemy")
            .Select(enemy => enemy.GetComponent<Collider>())
            .Where(collider => collider != null)
            .ToArray();

        foreach (Collider enemyCollider in enemyColliders)
        {
            Physics.IgnoreCollision(enemyCollider, GetComponent<Collider>());
        }

        // Ignore collision with the AOE Attack GameObject if it is set
        if (aoeAttackObject != null)
        {
            Collider aoeCollider = aoeAttackObject.GetComponent<Collider>();
            if (aoeCollider != null)
            {
                Physics.IgnoreCollision(aoeCollider, GetComponent<Collider>());
            }
        }

        // Set a random color to the projectile
        SetRandomColor();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is not the AOE Attack GameObject and does not have the tag "Damage", "Enemy", or "EnemyProjectile"
        if (collision.gameObject != aoeAttackObject && !collision.gameObject.CompareTag("Damage") && !collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("EnemyProjectile"))
        {
            // Destroy the projectile when it collides with any object except those with the tag "Damage", "Enemy", "EnemyProjectile", or the AOE Attack GameObject
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Ignore collision with the AOE Attack GameObject and objects that have the tag "Damage" or "Enemy"
        if (other.gameObject == aoeAttackObject || other.CompareTag("Damage") || other.CompareTag("Enemy"))
        {
            Physics.IgnoreCollision(other, GetComponent<Collider>());
        }
    }

    void SetRandomColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            renderer.material.color = randomColor;
        }
    }
}
