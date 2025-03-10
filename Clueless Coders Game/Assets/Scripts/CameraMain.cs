using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player object by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            // Update the camera's position to follow the player
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 20, playerTransform.position.z);
            originalPosition = transform.position; // Update the original position
        }
    }

    public void TriggerScreenShake(float duration, float magnitude)
    {
        StartCoroutine(ScreenShake(duration, magnitude));
    }

    private IEnumerator ScreenShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Reset the camera's position relative to the player
        if (playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 20, playerTransform.position.z);
        }
    }
}
