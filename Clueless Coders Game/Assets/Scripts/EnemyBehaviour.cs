using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the enemy moves towards the player
    public int health = 100; // Health of the enemy
    private Transform player;
    public ParticleSystem deathEffect; // Reference to the particle effect

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Move towards the player
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            // Reduce health when colliding with an object tagged as "Damage"
            health -= 10;

            // Check if health is less than or equal to 0
            if (health <= 0)
            {
                // Play the death effect
                if (deathEffect != null)
                {
                    Destroy(gameObject);
                    ParticleSystem effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                    effect.transform.parent = null; // Ensure the effect is not destroyed with the enemy
                    effect.Play();
                    Destroy(effect.gameObject, 0.5f); // Destroy the particle system after 0.2 seconds
                }

                // Destroy the enemy object
               // Destroy(gameObject);
            }
        }
    }
}
