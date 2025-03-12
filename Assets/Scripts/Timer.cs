// Timer.cs
using App;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    float remainingTime;
    float playtime = 0;
    public bool isCounting = false;
    public bool isCountingGameplay = false;
    [SerializeField] ScoreManager scoreManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

    public void GameplayCounter() {
        if(!isCountingGameplay) {
            isCountingGameplay = true;
            _upcount();
        }
    }

    public void StopGameplayCounter() {
        isCountingGameplay = false;
        scoreManager.SetPlaytime(playtime);
        CancelInvoke("_upcount");
    }

    void _upcount() {
        playtime++;
        Invoke("_upcount", 1f);
    }

    // public async Task SavePlaytimeToSupabase() {
    //     if(SupabaseStuff.Instance == null){
    //         Debug.LogError("Supabase Error: Instance is null.");
    //         return;
    //     }

    //     var user = SupabaseStuff.Instance.GetLoggedInUser();
    //     if(user == null) {
    //         Debug.LogError("no logged in user");
    //         return;
    //     }

    //     var newTimer 
    // }
}