using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start()
    {
        // Destroy the projectile after 5 seconds
        Destroy(gameObject, 5f);

        // Ignore collision with objects that have the tag "Player" or "EnemyProjectile"
        Collider[] collidersToIgnore = GameObject.FindGameObjectsWithTag("Player")
            .Concat(GameObject.FindGameObjectsWithTag("EnemyProjectile"))
            .Select(obj => obj.GetComponent<Collider>())
            .Where(collider => collider != null)
            .ToArray();

        foreach (Collider collider in collidersToIgnore)
        {
            Physics.IgnoreCollision(collider, GetComponent<Collider>());
        }

        // Set a random color to the projectile
        SetRandomColor();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object does not have the tag "Damage", "Player", or "EnemyProjectile"
        if (!collision.gameObject.CompareTag("Damage") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("EnemyProjectile"))
        {
            // Destroy the projectile when it collides with any object except those with the tag "Damage", "Player", or "EnemyProjectile"
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Ignore collision with objects that have the tag "Damage", "Player", or "EnemyProjectile"
        if (other.CompareTag("Damage") || other.CompareTag("Player") || other.CompareTag("EnemyProjectile"))
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

