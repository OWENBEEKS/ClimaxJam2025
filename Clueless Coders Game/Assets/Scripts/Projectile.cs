using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start()
    {
        // Destroy the projectile after 5 seconds
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object does not have the tag "Damage"
        if (!collision.gameObject.CompareTag("Damage"))
        {
            // Destroy the projectile when it collides with any object except those with the tag "Damage"
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Ignore collision with objects that have the tag "Damage"
        if (other.CompareTag("Damage"))
        {
            Physics.IgnoreCollision(other, GetComponent<Collider>());
        }
    }
}
