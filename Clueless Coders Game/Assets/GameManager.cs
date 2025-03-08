using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float startTime;
    public Text timerText;
    public GameObject enemyPrefab;
    private int instantiatedCount = 0;
    private const int maxInstantiatedCount = 50;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime;
        timerText.text = "TimeG: " + elapsedTime.ToString("F2") + " seconds";

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
        if (renderer != null)
        {
            renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}
