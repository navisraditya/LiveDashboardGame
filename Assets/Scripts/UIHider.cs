using UnityEngine;
using UnityEngine.UI;

public class UIHider : MonoBehaviour
{
    public Transform player;
    public CanvasGroup canvasGroup;
    private float hideThreshold = 5f;
    private float fadeSpeed = 1f;

    // Ensure this object persists across scenes
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if canvasGroup and player are valid before accessing them
        if (canvasGroup != null && player != null)
        {
            if (player.position.y > hideThreshold)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);
                if (canvasGroup.alpha <= 0f)
                {
                    DisableButtons();
                }
            }
        }
    }

    public void DisableButtons()
    {
        if (canvasGroup != null)
        {
            Button[] buttons = canvasGroup.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }
    }
}
