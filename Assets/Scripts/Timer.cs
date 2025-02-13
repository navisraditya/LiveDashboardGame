// Timer.cs
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    float remainingTime;
    public bool isCounting = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void BeginCouting(float duration)
    {
        if (!isCounting)
        {
            isCounting = true;
            remainingTime = duration;
            _tick(); // Start the countdown
        }
    }

    void _tick()
    {
        remainingTime--;
        if (remainingTime > 0)
        {
            Invoke("_tick", 1f);
        }
        else
        {
            isCounting = false;
        }
    }
}