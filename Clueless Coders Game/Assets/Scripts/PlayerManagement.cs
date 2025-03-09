using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagement : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public GameObject aoeAttack;
    public Text weaponSwapTimerText;
    public Text currentWeaponText; // New Text element for current weapon
    private bool canFire = true;
    private int weaponMode = 0;

    void Start()
    {
        if (aoeAttack != null)
        {
            aoeAttack.SetActive(false);
        }
        StartCoroutine(RandomizeWeaponMode());
        if (weaponSwapTimerText != null)
        {
            Outline outline = weaponSwapTimerText.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(1, -1);
            StartCoroutine(AnimateWeaponSwapText());
        }
        if (currentWeaponText != null)
        {
            Outline outline = currentWeaponText.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(1, -1);
            StartCoroutine(AnimateCurrentWeaponText());
        }
    }

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
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (worldPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position + direction;

        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, rotation);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        projectile.transform.rotation = Quaternion.LookRotation(direction);
    }

    void FireShotgun()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (worldPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position + direction;

        float spreadAngle = 6f;
        for (int i = -2; i <= 2; i++)
        {
            float angle = i * spreadAngle;
            Vector3 spreadDirection = Quaternion.Euler(0, angle, 0) * direction;
            Vector3 spreadPosition = transform.position + spreadDirection;
            Quaternion rotation = Quaternion.LookRotation(spreadDirection);
            GameObject projectile = Instantiate(projectilePrefab, spreadPosition, rotation);
            projectile.GetComponent<Rigidbody>().velocity = spreadDirection * projectileSpeed;
            projectile.transform.rotation = Quaternion.LookRotation(spreadDirection);
        }
    }

    void FireLaser()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (worldPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position + direction;

        int numberOfProjectiles = 10;
        float distanceBetweenProjectiles = 0.5f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Vector3 laserPosition = spawnPosition + direction * (i * distanceBetweenProjectiles);
            Quaternion rotation = Quaternion.LookRotation(direction);
            GameObject projectile = Instantiate(projectilePrefab, laserPosition, rotation);
            projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    void FireHomingMissile()
    {
        Vector3 direction = transform.forward; // Fire straight ahead
        Vector3 spawnPosition = transform.position + direction;

        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, rotation);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        projectile.transform.rotation = Quaternion.LookRotation(direction);

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

            string weaponName = "";
            switch (weaponMode)
            {
                case 0:
                    weaponName = "Single Projectile";
                    Debug.Log("Single projectile mode enabled.");
                    break;
                case 1:
                    weaponName = "Shotgun";
                    Debug.Log("Shotgun mode enabled.");
                    break;
                case 2:
                    weaponName = "AOE Attack";
                    Debug.Log("AOE Attack enabled.");
                    break;
                case 3:
                    weaponName = "Laser";
                    Debug.Log("Laser mode enabled.");
                    break;
                case 4:
                    weaponName = "Homing Missile";
                    Debug.Log("Homing Missile mode enabled.");
                    break;
            }

            if (currentWeaponText != null)
            {
                StartCoroutine(ScrambleText(currentWeaponText, weaponName));
            }
        }
    }

    IEnumerator ScrambleText(Text textElement, string finalText)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        int length = finalText.Length;
        float scrambleDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < scrambleDuration)
        {
            elapsedTime += Time.deltaTime;
            string scrambledText = "";
            for (int i = 0; i < length; i++)
            {
                scrambledText += chars[Random.Range(0, chars.Length)];
            }
            textElement.text = scrambledText;
            yield return null;
        }

        textElement.text = finalText;
    }

    IEnumerator AnimateWeaponSwapText()
    {
        while (true)
        {
            float hue = 0f;
            Vector3 originalPosition = weaponSwapTimerText.transform.position;
            while (true)
            {
                hue += Time.deltaTime * 0.5f; // Adjust the speed of color change
                if (hue > 1f) hue -= 1f;
                weaponSwapTimerText.color = Color.HSVToRGB(hue, 1f, 1f);

                float jump = Mathf.Sin(Time.time * 5f) * 5f; // Adjust the speed and height of the jump
                weaponSwapTimerText.transform.position = originalPosition + new Vector3(0, jump, 0);

                yield return null;
            }
        }
    }

    IEnumerator AnimateCurrentWeaponText()
    {
        while (true)
        {
            float hue = 0f;
            Vector3 originalPosition = currentWeaponText.transform.position;
            while (true)
            {
                hue += Time.deltaTime * 0.5f; // Adjust the speed of color change
                if (hue > 1f) hue -= 1f;
                currentWeaponText.color = Color.HSVToRGB(hue, 1f, 1f);

                float jump = Mathf.Sin(Time.time * 5f) * 5f; // Adjust the speed and height of the jump
                currentWeaponText.transform.position = originalPosition + new Vector3(0, jump, 0);

                yield return null;
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
