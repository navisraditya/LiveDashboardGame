using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject platformPrefab;
    public int platformCount = 300;
    public float fadeDuration = 1.25f;
    public bool isFrozen = false;
    public UIHider uiHider;

    private class FadingPlatform
    {
        public SpriteRenderer spriteRenderer;
        public float fadeTimer = 0f;
        public Rigidbody2D rb;  // Reference to the platform's Rigidbody2D (if applicable)
        public GameObject platformObject; // Reference to the platform GameObject
    }

    private FadingPlatform[] fadingPlatforms;
    private Transform playerTransform; // Reference to the player's transform

    void Start()
    {
        // Call PreGame to start the game in frozen state
        PreGame();

        // Get the player's transform
        if (playerPrefab != null)
        {
            playerTransform = playerPrefab.transform;
        }
    }

void Update()
{
    if (isFrozen)
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            UnfreezeGame();
        }
    }

    if (!isFrozen)
    {
        for (int i = 0; i < platformCount; i++)
        {
            if (fadingPlatforms[i] != null && fadingPlatforms[i].spriteRenderer != null)
            {
                FadingPlatform fadeData = fadingPlatforms[i];

                if (fadeData.fadeTimer < fadeDuration)
                {
                    fadeData.fadeTimer += Time.deltaTime;

                    float alpha = Mathf.Clamp01(fadeData.fadeTimer / fadeDuration);
                    Color color = fadeData.spriteRenderer.color;
                    color.a = alpha;
                    fadeData.spriteRenderer.color = color;
                }

                // Check if the camera has passed the platform
                if (fadeData.platformObject != null)
                {
                    float cameraBottomY = Camera.main.transform.position.y - (Camera.main.orthographicSize + 1f); // Adding a small buffer

                    if (fadeData.platformObject.transform.position.y < cameraBottomY)
                    {
                        Destroy(fadeData.platformObject);
                        fadingPlatforms[i] = null; // Mark as destroyed
                    }
                }
            }
        }
    }
}

    void PreGame()
    {
        // Freeze the game by setting objects to inactive (without deactivating them) and stopping movement
        playerPrefab.SetActive(true);
        platformPrefab.SetActive(true);

        // Add rigidbody reference for freezing physics
        fadingPlatforms = new FadingPlatform[platformCount];

        Vector3 spawnPosition = new Vector3();
        for (int i = 0; i < platformCount; i++)
        {
            spawnPosition.y += Random.Range(.5f, 2f);
            spawnPosition.x = Random.Range(-2.2f, 2.2f);
            spawnPosition.z = 0;

            GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

            SpriteRenderer spriteRenderer = platform.GetComponent<SpriteRenderer>();
            Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();  // Assuming you're using Rigidbody2D for physics-based platforms
            if (spriteRenderer != null)
            {
                fadingPlatforms[i] = new FadingPlatform
                {
                    spriteRenderer = spriteRenderer,
                    rb = rb,  // Save the reference to the Rigidbody2D
                    platformObject = platform, // Save the reference to the platform GameObject
                    fadeTimer = 0f
                };

                Color color = spriteRenderer.color;
                color.a = 0f; // Initially make the platforms invissible
                spriteRenderer.color = color;
            }

            // If there's a Rigidbody2D, set it to kinematic to freeze movement
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // Freeze the platform's movement by making it kinematic
            }
        }

        // Stop the game time (freeze everything except UI elements)
        Time.timeScale = 0;

        isFrozen = true;
    }

    void UnfreezeGame()
    {
        // Reactivate the player and platform objects
        playerPrefab.SetActive(true);
        platformPrefab.SetActive(true);
        uiHider.DisableButtons();
        // Start the game and resume time
        Time.timeScale = 1;

        // Reactivate the physics for the platforms
        for (int i = 0; i < platformCount; i++)
        {
            if (fadingPlatforms[i] != null && fadingPlatforms[i].rb != null)
            {
                fadingPlatforms[i].rb.bodyType = RigidbodyType2D.Dynamic; // Unfreeze the platform's movement
            }
        }

        // Platforms should now start fading in
        isFrozen = false;
    }
}