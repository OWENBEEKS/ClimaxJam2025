using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this line to use the UI namespace

public class PlayerManagement : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public GameObject aoeAttack; // Reference to the AOE Attack child object
    public Text weaponSwapTimerText; // Reference to the UI Text element
    private bool canFire = true;
    private int weaponMode = 0; // 0: Single Projectile, 1: Shotgun, 2: AOE Attack, 3: Laser, 4: Homing Missile

    // Start is called before the first frame update
    void Start()
    {
        if (aoeAttack != null)
        {
            aoeAttack.SetActive(false); // Ensure AOE Attack is initially disabled
        }
        StartCoroutine(RandomizeWeaponMode());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canFire)
        {
            switch (weaponMode)
            {
                case 0:
                    FireProjectile();
                    break;
                case 1:
                    FireShotgun();
                    break;
                case 2:
                    // AOE Attack logic can be added here if needed
                    break;
                case 3:
                    FireLaser();
                    break;
                case 4:
                    FireHomingMissile();
                    break;
            }
        }
    }

    void FireProjectile()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Set z to the player's z position in screen space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (worldPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position + direction;

        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, rotation);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        projectile.transform.rotation = Quaternion.LookRotation(direction); // Ensure the projectile faces the direction it is fired in
    }

    void FireShotgun()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Set z to the player's z position in screen space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (worldPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position + direction;

        float spreadAngle = 10f; // Angle between each projectile
        for (int i = -2; i <= 2; i++)
        {
            float angle = i * spreadAngle;
            Vector3 spreadDirection = Quaternion.Euler(0, angle, 0) * direction;
            Vector3 spreadPosition = transform.position + spreadDirection;
            Quaternion rotation = Quaternion.LookRotation(spreadDirection);
            GameObject projectile = Instantiate(projectilePrefab, spreadPosition, rotation);
            projectile.GetComponent<Rigidbody>().velocity = spreadDirection * projectileSpeed;
            projectile.transform.rotation = Quaternion.LookRotation(spreadDirection); // Ensure the projectile faces the direction it is fired in
        }
    }

    void FireLaser()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Set z to the player's z position in screen space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (worldPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position + direction;

        int numberOfProjectiles = 10; // Number of projectiles in the laser
        float distanceBetweenProjectiles = 0.5f; // Distance between each projectile

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Vector3 laserPosition = spawnPosition + direction * (i * distanceBetweenProjectiles);
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject projectile = Instantiate(projectilePrefab, laserPosition, rotation);
            projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
            projectile.transform.rotation = Quaternion.LookRotation(direction); // Ensure the projectile faces the direction it is fired in
        }
    }

    void FireHomingMissile()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Set z to the player's z position in screen space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (worldPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position + direction;

        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, rotation);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        projectile.transform.rotation = Quaternion.LookRotation(direction); // Ensure the projectile faces the direction it is fired in

        HomingMissile homingMissile = projectile.AddComponent<HomingMissile>();
        homingMissile.targetTag = "Enemy";
        homingMissile.speed = projectileSpeed;
    }

    IEnumerator RandomizeWeaponMode()
    {
        while (true)
        {
            float timer = 10f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                if (weaponSwapTimerText != null)
                {
                    weaponSwapTimerText.text = "Weapon swap in: " + Mathf.Ceil(timer).ToString() + "s";
                }
                yield return null;
            }

            weaponMode = Random.Range(0, 5);

            if (aoeAttack != null)
            {
                aoeAttack.SetActive(weaponMode == 2);
            }

            switch (weaponMode)
            {
                case 0:
                    Debug.Log("Single projectile mode enabled.");
                    break;
                case 1:
                    Debug.Log("Shotgun mode enabled.");
                    break;
                case 2:
                    Debug.Log("AOE Attack enabled.");
                    break;
                case 3:
                    Debug.Log("Laser mode enabled.");
                    break;
                case 4:
                    Debug.Log("Homing Missile mode enabled.");
                    break;
            }
        }
    }
}

public class HomingMissile : MonoBehaviour
{
    public string targetTag;
    public float speed;
    private Transform target;

    void Start()
    {
        target = FindClosestEnemy();
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            GetComponent<Rigidbody>().velocity = direction * speed;
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }
}
