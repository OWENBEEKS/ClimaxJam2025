using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
<<<<<<< Updated upstream
    public float minSpeed = 1.0f; // Minimum speed at which the enemy can move
    public float maxSpeed = 5.0f; // Maximum speed at which the enemy can move
    private float speed; // Speed at which the enemy moves towards the player
=======
    public GameObject projectilePrefab;
    public float speed = 2.0f; // Speed at which the enemy moves towards the player
    public float shotSpeed = 5.0f; // Speed of the projectile  
>>>>>>> Stashed changes
    public int health = 100; // Health of the enemy
    private Transform player;
    public ParticleSystem deathEffect; // Reference to the particle effect
    public float shootInterval = 2.0f; // Interval between shots
    private float shootTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
<<<<<<< Updated upstream
        speed = Random.Range(minSpeed, maxSpeed); // Randomize the speed
        StartCoroutine(IncreaseHealthOverTime()); // Start the coroutine to increase health
=======
        shootTimer = shootInterval;
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Move towards the player
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);

            // Handle shooting
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                ShootProjectile();
                shootTimer = shootInterval;
            }
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab != null && player != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector3 direction = (player.position - transform.position).normalized;
            projectile.GetComponent<Rigidbody>().velocity = direction * shotSpeed;
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
                    ParticleSystem effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                    effect.transform.parent = null; // Ensure the effect is not destroyed with the enemy
                    effect.Play();
                    Destroy(effect.gameObject, 0.5f); // Destroy the particle system after 0.5 seconds
                }

                // Play the explosion sound from the "Death Sound" GameObject
                PlayExplosionSound();

                // Destroy the enemy object
                Destroy(gameObject);
            }
        }
    }

    private void PlayExplosionSound()
    {
        GameObject deathSoundObject = GameObject.Find("Death Sound");
        if (deathSoundObject != null)
        {
            AudioSource audioSource = deathSoundObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }

    private IEnumerator IncreaseHealthOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(30); // Wait for 60 seconds
            health += 10; // Increase health by 10
        }
    }
}
