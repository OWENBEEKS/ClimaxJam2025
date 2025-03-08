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
        // Destroy the projectile when it collides with any object
        Destroy(gameObject);
    }
}
