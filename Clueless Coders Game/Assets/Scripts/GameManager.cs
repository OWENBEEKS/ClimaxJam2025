using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float startTime;
    private float timeSinceLastChange;
    private float enemyChangeTime;
    public Text timerText;
    public GameObject enemyPrefab;
    public GameObject floor;
    public List<Material> materials;
    private int instantiatedCount = 0;
    private const int maxInstantiatedCount = 50;
    private TimeSave timeSave;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        timeSinceLastChange = Time.time;
        enemyChangeTime = Time.time;
        timeSave = FindObjectOfType<TimeSave>();
        if (timerText != null)
        {
            StartCoroutine(AnimateTimerText());
        }
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime;
        timerText.text = "Time: " + elapsedTime.ToString("F2") + " seconds";

        if (instantiatedCount < maxInstantiatedCount)
        {
            if (elapsedTime >= 10 && elapsedTime < 20)
            {
                StartCoroutine(InstantiateAndChangeColor());
            }
            else if (elapsedTime >= 20 && elapsedTime < 30)
            {
                StartCoroutine(InstantiateAndChangeColor());
            }
            else if (elapsedTime >= 30)
            {
                StartCoroutine(InstantiateAndChangeColor());
            }
        }
        if (Time.time - timeSinceLastChange >= 7)
        {
            ChangeMaterialColor(floor);
            timeSinceLastChange = Time.time;
        }
        if (Time.time - enemyChangeTime >= 7)
        {
            ChangeMaterialColor(enemyPrefab);
            enemyChangeTime = Time.time;
        }

        // Example condition to save time
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timeSave.SaveTime(elapsedTime);
        }
    }

    IEnumerator InstantiateAndChangeColor()
    {
        yield return new WaitForSeconds(60); // Wait for 60 seconds

        GameObject newObject = Instantiate(enemyPrefab);
        instantiatedCount++;
        ChangeMaterialColor(newObject);
    }

    void ChangeMaterialColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && materials.Count > 0)
        {
            Material randomMaterial = materials[Random.Range(0, materials.Count)];
            renderer.material = randomMaterial;
        }
    }

    IEnumerator AnimateTimerText()
    {
        while (true)
        {
            float hue = 0f;
            Vector3 originalPosition = timerText.transform.position;
            while (true)
            {
                hue += Time.deltaTime * 0.5f; // Adjust the speed of color change
                if (hue > 1f) hue -= 1f;
                timerText.color = Color.HSVToRGB(hue, 1f, 1f);

                float jump = Mathf.Sin(Time.time * 5f) * 5f; // Adjust the speed and height of the jump
                timerText.transform.position = originalPosition + new Vector3(0, jump, 0);

                yield return null;
            }
        }
    }
}
