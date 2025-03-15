// using TMPro;
// using UnityEngine;
// using System.Collections.Generic;
// using Postgrest;
// using System.Linq;
// using System;
// using App;
// using Cysharp.Threading.Tasks;

// public class ScoreManager : MonoBehaviour
// {
//     public static ScoreManager Instance { get; private set; }

//     public TMP_Text scoreUI;

//     private int score = 0;
//     private float playtime = 0;

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject); // Prevent duplicate ScoreManager instances
//         }
//     }

//     public void IncScore()
//     {
//         score++;
//         UpdateScoreUI();
//     }

//     private void UpdateScoreUI()
//     {
//         if (scoreUI != null)
//         {
//             scoreUI.text = score.ToString();
//         }
//         else
//         {
//             Debug.LogWarning("Score UI is not assigned.");
//         }
//     }

//     public async UniTask SaveScoreToSupabase()
//     {
//         if (SupabaseStuff.Instance == null)
//         {
//             Debug.LogError("Supabase error: Instance is null.");
//             return;
//         }

//         var user = SupabaseStuff.Instance.GetLoggedInUser();
//         if (user == null)
//         {
//             Debug.Log("No Active Player. Score will not be saved.");
//             return;
//         }

//         var newScore = new ScoreModel
//         {
//             PlayerId = user.UserMetadata["username"].ToString(),
//             ScoreValue = score,
//             playtime = playtime,
//         };

//         try
//         {
//             var response = await SupabaseStuff.Instance.GetSupabaseClient()
//                 .From<ScoreModel>()
//                 .Insert(newScore);

//             if (response != null && response.Models != null)
//             {
//                 Debug.Log("Score saved.");
//             }
//             else
//             {
//                 Debug.Log("Saving score failed.");
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"Error saving score: {e.Message}");
//         }
//     }

//     public async UniTask<List<ScoreModel>> FetchScores(int limit = 20)
//     {
//         if (SupabaseStuff.Instance == null)
//         {
//             Debug.LogError("Supabase error: Instance is null.");
//             return new List<ScoreModel>();
//         }

//         var response = await SupabaseStuff.Instance.GetSupabaseClient()
//             .From<ScoreModel>()
//             .Select("*")
//             .Order("score", Constants.Ordering.Descending)
//             .Limit(limit)
//             .Get();

//         if (response != null)
//         {
//             return response.Models.ToList();
//         }
//         else
//         {
//             Debug.Log("No scores found.");
//             return new List<ScoreModel>();
//         }
//     }
//     public void SetPlaytime(float timerPlaytime) {
//         playtime = timerPlaytime;
//     }
// }


using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TMP_Text scoreUI;

    private int score = 0;
    private float playtime = 0;

    private const string SUPABASE_URL = "https://rbmxqlqzyemtwsajfjtw.supabase.co";
    private const string SUPABASE_API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJibXhxbHF6eWVtdHdzYWpmanR3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzcwMDgyNjcsImV4cCI6MjA1MjU4NDI2N30.N2-ULM2_1zc_yCo3zoYlolIZhX8OPnixsILHqhZxTO8";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate ScoreManager instances
        }
    }

    public void IncScore()
    {
        score++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreUI != null)
        {
            scoreUI.text = score.ToString();
        }
        else
        {
            Debug.LogWarning("Score UI is not assigned.");
        }
    }

    public async UniTask SaveScoreToSupabase()
    {
        // Fetch the logged-in user (if any)
        var user = await GetLoggedInUser();
        if (user == null)
        {
            Debug.Log("No Active Player. Score will not be saved.");
            return;
        }

        // Prepare the score data
        var scoreData = new ScoreModel
        {
            PlayerId = user.UserMetadata["username"].ToString(),
            ScoreValue = score,
            playtime = playtime,
        };

        // Convert the score data to JSON
        string jsonData = JsonUtility.ToJson(scoreData);

        // Send the score data to Supabase
        string url = $"{SUPABASE_URL}/rest/v1/scores";
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("apikey", SUPABASE_API_KEY);
            webRequest.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");

            await webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error saving score: " + webRequest.error);
                Debug.LogError("Response: " + webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log("Score saved successfully.");
            }
        }
    }

    private async UniTask<User> GetLoggedInUser()
    {
        string url = $"{SUPABASE_URL}/auth/v1/user";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("apikey", SUPABASE_API_KEY);
            webRequest.SetRequestHeader("Authorization", $"Bearer {SUPABASE_API_KEY}");

            await webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching user: " + webRequest.error);
                return null;
            }
            else
            {
                var user = JsonUtility.FromJson<User>(webRequest.downloadHandler.text);
                return user;
            }
        }
    }

    public async UniTask<List<ScoreModel>> FetchScores(int limit = 20)
    {
        string url = $"{SUPABASE_URL}/rest/v1/scores?select=id,player_id,score,playtime&order=score.desc&limit={limit}";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("apikey", SUPABASE_API_KEY);

            await webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching scores: " + webRequest.error);
                return new List<ScoreModel>();
            }
            else
            {
                // Wrap the JSON array in an object to make it compatible with JsonUtility
                string jsonResponse = $"{{\"scores\":{webRequest.downloadHandler.text}}}";
                var wrapper = JsonUtility.FromJson<ScoreWrapper>(jsonResponse);
                return wrapper.scores;
            }
        }
    }

    public void SetPlaytime(float timerPlaytime)
    {
        playtime = timerPlaytime;
    }

    [System.Serializable]
    public class ScoreModel
    {
        public string PlayerId;
        public int ScoreValue;
        public float playtime;
    }

    [System.Serializable]
    public class User
    {
        public string Id;
        public string Email;
        public Dictionary<string, object> UserMetadata;
    }

    [System.Serializable]
    private class ScoreWrapper
    {
        public List<ScoreModel> scores;
    }
}