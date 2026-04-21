using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float minSpeed = 1.0f; // Minimum speed at which the enemy can move
    public float maxSpeed = 5.0f; // Maximum speed at which the enemy can move
    //public GameObject projectilePrefab;
    public float speed = 2.0f; // Speed at which the enemy moves towards the player
    //public float shotSpeed = 5.0f; // Speed of the projectile  
    public int health = 100; // Health of the enemy
    private Transform player;
    public ParticleSystem deathEffect; // Reference to the particle effect
    public float shootInterval = 2.0f; // Interval between shots
    //private float shootTimer;
    public float shakeDuration = 0.2f; // Duration of the shake
    public float shakeMagnitude = 0.3f; // Magnitude of the shake

    public GameObject xpPrefab; // Reference to the XP prefab

    private CameraMain cameraMain;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        speed = Random.Range(minSpeed, maxSpeed); // Randomize the speed
        StartCoroutine(IncreaseHealthOverTime()); // Start the coroutine to increase health
        //shootTimer = shootInterval;

        // Find the CameraMain script
        cameraMain = Camera.main.GetComponent<CameraMain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Move towards the player
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);

            // Rotate to look at the player
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

            // Handle shooting
            //shootTimer -= Time.deltaTime;
            //if (shootTimer <= 0)
           // {
            //    ShootProjectile();
            //    shootTimer = shootInterval;
           // }
        }
    }

    //private void ShootProjectile()
    //{
    //    if (projectilePrefab != null && player != null)
    //    {
    //        Vector3 direction = (player.position - transform.position).normalized;
    //        Quaternion rotation = Quaternion.LookRotation(direction);

    //        GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation);
    //        projectile.GetComponent<Rigidbody>().velocity = direction * shotSpeed;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            // Check the LevelManager for the current player damage, default to 10 if missing
            int damageTaken = 10;
            if (LevelManager.Instance != null)
            {
                damageTaken = LevelManager.Instance.playerDamage;
            }

            health -= damageTaken;

            if (health <= 0)
            {
                // Play the death effect
                if (deathEffect != null)
                {
                    ParticleSystem effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
                    effect.transform.parent = null;
                    effect.Play();
                    Destroy(effect.gameObject, 0.5f);
                }

                // Play the explosion sound
                PlayExplosionSound();

                // Trigger screen shake
                if (cameraMain != null)
                {
                    cameraMain.TriggerScreenShake(shakeDuration, shakeMagnitude);
                }

                // Spawn XP objects
                if (xpPrefab != null)
                {
                    Vector3[] offsets = { new Vector3(0.2f, 0, 0), new Vector3(-0.2f, 0, 0) };
                    foreach (var offset in offsets)
                    {
                        GameObject xp = Instantiate(xpPrefab, transform.position + offset, Quaternion.identity);
                        Rigidbody rb = xp.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            // Generate a random direction
                            Vector3 randomDir = new Vector3(
                                Random.Range(-1f, 1f),
                                Random.Range(0.3f, 0.7f),
                                Random.Range(-1f, 1f)
                            ).normalized;
                            float forceMagnitude = 2.0f;
                            rb.AddForce(randomDir * forceMagnitude, ForceMode.Impulse);
                        }
                    }
                }

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
            yield return new WaitForSeconds(15); // Wait for 30 seconds
            health += 5; // Increase health by 10
            //Figure out scaling for this.
        }
    }
}
