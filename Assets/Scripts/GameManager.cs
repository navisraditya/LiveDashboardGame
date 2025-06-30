using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject platformPrefab;
    public static int platformCount = 300;
    public int level1 = platformCount / 2;
    public int level2 = platformCount * 2 / 3;
    public int level3;
    
    public float fadeDuration = 1.25f;
    public bool isFrozen = false;
    public UIHider uiHider;


    public float minPlatformYDistEz = 0.5f;
    public float maxPlatformYDistEz = 2.0f;
    public float minPlatformYDistMed = 1.5f;
    public float maxPlatformYDistMed = 3.0f;
    public float minPlatformYDistHard = 2.5f;
    public float maxPlatformYDistHard = 4.0f;


    private float currMinYDist;
    private float currMaxYDist;
    private float lastCamPos;
    private int scoreIncVal = 1;
    private int scoreLevel = 1;


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
                lastCamPos = Camera.main.transform.position.y;
                UnfreezeGame();
            }
        }

        if (!isFrozen)
        {
            ScoreByCamY();
            DestroyPlatform();
        }
    }

    void DestroyPlatform()
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

    void ScoreByCamY()
    {
            float currCamY = Camera.main.transform.position.y;
            float yMovement = currCamY - lastCamPos;

            if (yMovement > 0)
            {
                ScoreManager.Instance.IncScore(scoreIncVal);
            }
            lastCamPos = currCamY;
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

            if (i < level1)
            {
                scoreLevel = 1;
                currMinYDist = minPlatformYDistEz;
                currMaxYDist = maxPlatformYDistEz;
            }
            else if (i < level2)
            {
                scoreLevel = 2;
                currMinYDist = minPlatformYDistMed;
                currMaxYDist = maxPlatformYDistMed;
            }
            else
            {
                scoreLevel = 3;
                currMinYDist = minPlatformYDistHard;
                currMaxYDist = maxPlatformYDistHard;
            }

            spawnPosition.y += Random.Range(currMinYDist, currMaxYDist);
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

        Timer.Instance.GameplayCounter();

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

    // public void DestroyPlatforms()
    // {
    //     for (int i = 0; i < fadingPlatforms.Length; i++)
    //     {
    //         if (fadingPlatforms[i] != null && fadingPlatforms[i].platformObject != null)
    //         {
    //             Destroy(fadingPlatforms[i].platformObject); // Destroy the platform GameObject
    //             fadingPlatforms[i] = null; // Clear the reference in the array
    //         }
    //     }
    // }
}