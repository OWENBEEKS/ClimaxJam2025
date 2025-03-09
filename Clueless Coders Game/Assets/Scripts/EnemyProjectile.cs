using System.Linq;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    void Start()
    {
        // Destroy the projectile after 5 seconds
        Destroy(gameObject, 5f);

        // Ignore collision with objects that have the tag "Player"
        Collider[] playerColliders = GameObject.FindGameObjectsWithTag("Enemy")
            .Select(player => player.GetComponent<Collider>())
            .Where(collider => collider != null)
            .ToArray();

        foreach (Collider playerCollider in playerColliders)
        {
            Physics.IgnoreCollision(playerCollider, GetComponent<Collider>());
        }

        // Set a random color to the projectile
        SetRandomColor();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object does not have the tag "Damage", "Player", or "AOE Attack"
        if (!collision.gameObject.CompareTag("Damage") && !collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("EnemyProjectile") && !collision.gameObject.CompareTag("AOE Attack"))
        {
            // Destroy the projectile when it collides with any object except those with the tag "Damage", "Player", or "AOE Attack"
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Ignore collision with objects that have the tag "Damage", "Player", or "AOE Attack"
        if (other.CompareTag("Damage") || other.CompareTag("Enemy") || other.CompareTag("AOE Attack"))
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
