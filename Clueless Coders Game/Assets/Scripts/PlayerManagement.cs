using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public GameObject aoeAttack; // Reference to the AOE Attack child object
    private bool canFire = true;
    private int weaponMode = 0; // 0: Single Projectile, 1: Shotgun, 2: AOE Attack, 3: Laser

    // Start is called before the first frame update
    void Start()
    {
        if (aoeAttack != null)
        {
            aoeAttack.SetActive(false); // Ensure AOE Attack is initially disabled
        }
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
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            CycleWeaponMode();
        }
    }

    void FireProjectile()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Set z to the player's z position in screen space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (worldPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position + direction;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
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
            GameObject projectile = Instantiate(projectilePrefab, spreadPosition, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = spreadDirection * projectileSpeed;
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
            GameObject projectile = Instantiate(projectilePrefab, laserPosition, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        }
    }

    void CycleWeaponMode()
    {
        weaponMode = (weaponMode + 1) % 4;

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
        }
    }
}
