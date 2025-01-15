using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject platformPrefab;
    // public GameObject scoreUI;
    public int platformCount = 300;
    public float fadeDuration = 1.25f;
    public bool isFrozen = false;
    public UIHider uiHider;

    private class FadingPlatform
    {
        public SpriteRenderer spriteRenderer;
        public float fadeTimer = 0f;
        public Rigidbody2D rb;  // Reference to the platform's Rigidbody2D (if applicable)
    }

    private FadingPlatform[] fadingPlatforms;

    void Start()
    {
        // Call PreGame to start the game in frozen state
        PreGame();
    }

    void Update()
    {
        // If the game is frozen, check for input to unfreeze
        if (isFrozen)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                UnfreezeGame();
            }
        }

        // Fade platforms in (if not frozen)
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
                    fadeTimer = 0f
                };

                Color color = spriteRenderer.color;
                color.a = 0f; // Initially make the platforms invisible
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
        // scoreUI.SetActive(true);
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
