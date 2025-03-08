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

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        timeSinceLastChange = Time.time;
        enemyChangeTime = Time.time;
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
                InstantiateAndChangeColor();
            }
            else if (elapsedTime >= 20 && elapsedTime < 30)
            {
                InstantiateAndChangeColor();
            }
            else if (elapsedTime >= 30)
            {
                InstantiateAndChangeColor();
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
    }

    void InstantiateAndChangeColor()
    {
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
}
