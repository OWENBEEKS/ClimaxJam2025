using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Text LevelText;
    public Text XpAmountText;
    public int Level = 1;
    public int XpAmount;
    public int XpAmountCap;

    [Header("Player Stats")]
    public float playerDamage = 10f;      // Changed to float for accurate percentage scaling

    [Header("XP Magnet Settings")]
    public Transform playerTransform;
    public float magnetRadius = 5f;
    public float magnetSpeed = 15f;
    public float collectionRadius = 0.5f;
    public int xpPerPickup = 1;

    [Header("Level Up UI")]
    public GameObject levelUpPanel;        // Assign a UI Panel in the inspector
    public Button[] upgradeButtons;        // Assign 3 UI Buttons here
    public Text[] upgradeButtonTexts;      // Assign the Text components of those 3 buttons here

    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Try to find the player automatically if not assigned in the inspector
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        // Ensure the level up panel is hidden at start
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update UI
        LevelText.text = Level.ToString();
        XpAmountText.text = "" + XpAmount + "/" + XpAmountCap + "";

        // Handle XP Suction
        if (playerTransform != null)
        {
            AttractXP();
        }
        
        // Handle Level up
        if (XpAmount >= XpAmountCap)
        {
            LevelUp();
        }
    }

    void AttractXP()
    {
        // Find all colliders within the magnet radius around the player
        Collider[] nearbyObjects = Physics.OverlapSphere(playerTransform.position, magnetRadius);

        foreach (Collider col in nearbyObjects)
        {
            // Check if the object is called "XP" or tagged "XP"
            if (col.CompareTag("XP") || col.gameObject.name.Contains("XP"))
            {
                Transform xpObj = col.transform;

                // Move the XP object towards the player
                xpObj.position = Vector3.MoveTowards(xpObj.position, playerTransform.position, magnetSpeed * Time.deltaTime);

                // Check distance to see if it's close enough to be collected
                if (Vector3.Distance(xpObj.position, playerTransform.position) <= collectionRadius)
                {
                    // Add the XP amount and destroy the object
                    XpAmount += xpPerPickup;
                    Destroy(xpObj.gameObject);
                }
            }
        }
    }

    void LevelUp()
    {
        Level++;
        XpAmount -= XpAmountCap;         // Carry over remaining XP
        XpAmountCap = Mathf.RoundToInt(XpAmountCap * 1.5f); // Increase the cap for the next level

        ShowLevelUpOptions();
    }

    void ShowLevelUpOptions()
    {
        // Pause the game
        Time.timeScale = 0f;

        // Show the panel
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
        }

        // Generate 3 random percentage increases for damage
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            // Randomly choose an increase between 5% and 25%
            int percentIncrease = Random.Range(15, 100); 

            if (upgradeButtonTexts[i] != null)
            {
                upgradeButtonTexts[i].text = "+" + percentIncrease + "% Damage";
            }

            // Remove previous listeners and add the new chosen upgrade
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => ApplyUpgrade(percentIncrease));
        }
    }

    public void ApplyUpgrade(int percentIncrease)
    {
        // Calculate and apply the percentage increase to current damage
        float multiplier = 1f + (percentIncrease / 100f);
        playerDamage *= multiplier;

        // Hide the panel
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }

        // Unpause the game
        Time.timeScale = 1f;
    }
}
