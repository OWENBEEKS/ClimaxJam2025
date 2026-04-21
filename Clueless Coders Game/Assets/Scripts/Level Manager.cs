using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    //Very basic levelingg up system that increases damage, will need to add more to get the player to pick and figure out balancing.
    public Text LevelText;
    public Text XpAmountText;
    public int Level = 1;
    public int XpAmount;
    public int XpAmountCap;

    [Header("Player Stats")]
    public int playerDamage = 10;         // Starting damage amount
    public int damageIncreasePerLevel = 5; // How much damage increases per level

    [Header("XP Magnet Settings")]
    public Transform playerTransform;
    public float magnetRadius = 5f;       // Distance at which XP starts moving towards player
    public float magnetSpeed = 15f;       // Speed at which XP moves
    public float collectionRadius = 0.5f; // Distance at which XP is collected
    public int xpPerPickup = 1;           // Amount of XP per object

    public static LevelManager Instance; // Singleton reference

    private void Awake()
    {
        // Set up a basic singleton so other scripts can access damage easily
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
        
        // Increase the player's damage
        playerDamage += damageIncreasePerLevel;
    }
}
