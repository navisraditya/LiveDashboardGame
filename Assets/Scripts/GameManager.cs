using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject platformPrefab;
    [SerializeField] GameObject FadingCanvas;
    public int platformCount = 300;
    public int level1;
    public int level2;
    public int level3;
    [SerializeField] int jumpForce1 = 10;
    [SerializeField] int jumpForce2 = 50;
    [SerializeField] int jumpForce3 = 100;


    public float fadeDuration = 1.25f;
    public bool isFrozen = false;
    public UIHider uiHider;


    public float minPlatformYDistEz = 0.5f;
    public float maxPlatformYDistEz = 2.0f;
    public float minPlatformYDistMed = 1.5f;
    public float maxPlatformYDistMed = 3.0f;
    public float minPlatformYDistHard = 2.5f;
    public float maxPlatformYDistHard = 4.0f;


    public float minXOffset = 0.5f;
    public float maxXOffset = 2.0f;
    public float bigGapChance = 0.1f;
    public float bigGapMinX = 2.5f;
    public float bigGapMaxX = 3.5f;


    private float currMinYDist;
    private float currMaxYDist;
    private float lastCamPos;
    public int latestPlatformIdx = 0;
    private Vector3 lastSpawnedPos;


    private BGM? currentPLayingBGM = null;
    public int currLevel = 1;


    private class FadingPlatform
    {
        public SpriteRenderer spriteRenderer;
        public float fadeTimer = 0f;
        public Rigidbody2D rb;
        public GameObject platformObject;
        public float individualFadeDuration;
    }

    private List<FadingPlatform> activePlatforms;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        activePlatforms = new List<FadingPlatform>();
        playerPrefab.SetActive(true);

        lastSpawnedPos = playerPrefab.transform.position;
        SpawnSinglePlatform(lastSpawnedPos, true);

        for (int i = 0; i < platformCount; i++)
        {
            SpawnSinglePlatform();
        }
        lastCamPos = Camera.main.transform.position.y;

        Time.timeScale = 0;
        isFrozen = true;

        if (SoundPrefab.Instance != null && currentPLayingBGM != BGM.MainBGM)
        {
            SoundPrefab.Instance.PlayBGM(BGM.MainBGM);
            currentPLayingBGM = BGM.MainBGM;
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
            LevelManager();
            ManageActivePlatformsAndSpawnNew();
        }
    }

    void LevelManager()
    {
        BGM desiredBGM = BGM.level1;

        if (currLevel == 1)
        {
            desiredBGM = BGM.level1;
        }
        else if (currLevel == 2)
        {
            desiredBGM = BGM.level2;
        }
        else
        {
            desiredBGM = BGM.level3;
        }

        if (SoundPrefab.Instance != null && currentPLayingBGM != desiredBGM)
        {
            SoundPrefab.Instance.PlayBGM(desiredBGM);
            currentPLayingBGM = desiredBGM;
        }
    }

    void ManageActivePlatformsAndSpawnNew()
    {
        float cameraBottomY = Camera.main.transform.position.y - (Camera.main.orthographicSize + 1f);

        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            FadingPlatform fadeData = activePlatforms[i];

            if (fadeData.platformObject == null)
            {
                activePlatforms.RemoveAt(i);
                continue;
            }

            if (fadeData.fadeTimer < fadeDuration)
            {
                fadeData.fadeTimer += Time.deltaTime;
                float alpha = Mathf.Clamp01(fadeData.fadeTimer / fadeDuration);
                Color color = fadeData.spriteRenderer.color;
                color.a = alpha;
                fadeData.spriteRenderer.color = color;
            }

            if (fadeData.platformObject.transform.position.y < cameraBottomY)
            {
                if (fadeData.platformObject.name != "Platform")
                {
                    Destroy(fadeData.platformObject);
                    activePlatforms.RemoveAt(i);
                }
            }
        }

        if (activePlatforms.Count < platformCount)
        {
            int platformsNeeded = platformCount - activePlatforms.Count;
            for (int i = 0; i < platformsNeeded; i++)
            {
                SpawnSinglePlatform();
            }
        }
    }

    void SpawnSinglePlatform(Vector3? explicitSpawnPosition = null, bool isStartingPlatform = false)
    {
        Vector3 spawnPosition;
        if (explicitSpawnPosition.HasValue)
        {
            spawnPosition = explicitSpawnPosition.Value;
        }
        else
        {
            if (latestPlatformIdx < level1)
            {
                currLevel = 1;
                platformCount = 15;
                currMinYDist = minPlatformYDistEz;
                currMaxYDist = maxPlatformYDistEz;
            }
            else if (latestPlatformIdx < level2)
            {
                currLevel = 2;
                platformCount = 10;
                currMinYDist = minPlatformYDistMed;
                currMaxYDist = maxPlatformYDistMed;
            }
            else if (latestPlatformIdx < level3)
            {
                currLevel = 3;
                platformCount = 5;
                currMinYDist = minPlatformYDistHard;
                currMaxYDist = maxPlatformYDistHard;
            }
            else
            {
                currLevel = 3;
                float[] minYDistList = { minPlatformYDistEz, minPlatformYDistMed, minPlatformYDistHard };
                float[] maxYDistList = { maxPlatformYDistEz, maxPlatformYDistMed, maxPlatformYDistHard };

                currMinYDist = UnityEngine.Random.Range(minYDistList[0], minYDistList[minYDistList.Count() - 1]);
                currMaxYDist = UnityEngine.Random.Range(maxYDistList[0], maxYDistList[maxYDistList.Count() -1]);
            }

            spawnPosition.y = lastSpawnedPos.y + UnityEngine.Random.Range(currMinYDist, currMaxYDist);

            float currentMinXOffset = minXOffset;
            float currentMaxXOffset = maxXOffset;

            if (UnityEngine.Random.value < bigGapChance)
            {
                currentMinXOffset = bigGapMinX;
                currentMaxXOffset = bigGapMaxX;
            }

            float horizontalOffset = UnityEngine.Random.Range(currentMinXOffset, currentMaxXOffset);
            if (UnityEngine.Random.value > 0.5f)
            {
                horizontalOffset *= -1;
            }

            spawnPosition.x = lastSpawnedPos.x + horizontalOffset;

            float screenHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -screenHalfWidth + 0.7f, screenHalfWidth - 0.7f);

            spawnPosition.z = 0;
        }

        GameObject platformGO = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        if (isStartingPlatform)
        {
            platformGO.name = "Platform";
        }
        else
        {
            platformGO.name = platformPrefab.name + "(Clone)";
        }

        Platform platformScript = platformGO.GetComponent<Platform>();
        if (platformScript != null)
        {
            if (latestPlatformIdx < level1)
            {
                platformScript.jumpForce = jumpForce1;
            }
            else if (latestPlatformIdx < level2)
            {
                platformScript.jumpForce = jumpForce2;
            }
            else if (latestPlatformIdx < level3)
            {
                platformScript.jumpForce = jumpForce3;
            }
        }

        SpriteRenderer spriteRenderer = platformGO.GetComponent<SpriteRenderer>();
        Rigidbody2D rb = platformGO.GetComponent<Rigidbody2D>();

        if (spriteRenderer != null)
        {
            float individualFadeTime = fadeDuration; 

            FadingPlatform newFadingPlatform = new FadingPlatform
            {
                spriteRenderer = spriteRenderer,
                rb = rb,
                platformObject = platformGO,
                fadeTimer = 0f,
                individualFadeDuration = individualFadeTime
            };
            activePlatforms.Add(newFadingPlatform); 

            Color color = spriteRenderer.color;
            color.a = 0f; // Start invisible
            spriteRenderer.color = color;
        }

        if (rb != null)
        {
            rb.bodyType = isFrozen ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        }

        if (!explicitSpawnPosition.HasValue)
        {
            lastSpawnedPos = spawnPosition;
        }
        latestPlatformIdx++; 
    }

    public void InactivePlatformAndPlayer()
    {
        FadingCanvas.SetActive(false);
        for (int i = 0; i < activePlatforms.Count(); i++)
        {
            FadingPlatform currPlatform = activePlatforms[i];

            currPlatform.platformObject.SetActive(false);
            playerPrefab.SetActive(false);
        }
    }

    void UnfreezeGame()
    {
        playerPrefab.SetActive(true);

        uiHider.DisableButtons();
        Time.timeScale = 1;

        Timer.Instance.GameplayCounter();

        foreach (FadingPlatform fp in activePlatforms)
        {
            if (fp.rb != null)
            {
                fp.rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        lastCamPos = Camera.main.transform.position.y;
        isFrozen = false;

        if (SoundPrefab.Instance != null && currentPLayingBGM != BGM.level1)
        {
            SoundPrefab.Instance.PlayBGM(BGM.level1);
            currentPLayingBGM = BGM.level1;
        }
    }
}