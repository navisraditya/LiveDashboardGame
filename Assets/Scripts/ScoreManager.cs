using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using App;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Unity.VisualScripting;
using System.Text;
using static App.SupabaseStuff;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance { get; private set; }

    public TMP_Text scoreUI;

    private int score = 0;
    private float playtime = 0;

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

    // public async Task SaveScoreToSupabase()
    // {
    //     if (SupabaseStuff.Instance == null)
    //     {
    //         Debug.LogError("Supabase error: Instance is null.");
    //         return;
    //     }

    //     var user = await SupabaseStuff.Instance.GetLoggedInUser();
    //     if (user == null)
    //     {
    //         Debug.Log("No Active Player. Score will not be saved.");
    //         return;
    //     }

    //     // if(user.UserMetadata != null && user.UserMetadata.Equals("username")) {4
    //     if(!user.UserMetadata.IsUnityNull()) {
    //         Debug.Log("ada usermetadata");
    //         string username = user.UserMetadata.username.ToString();
    //         var newScore = new ScoreModel
    //         {
    //             player_id = username,
    //             score = score,
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
    // }

    public async Task SaveScoreToSupabase()
    {
        string url = $"{SupabaseStuff.Instance.GetURL()}/rest/v1/scores";
        

        SupabaseStuff.User user =  SupabaseStuff.Instance.GetLoggedInUser();
        if(user == null) {
            Debug.Log("user kosong gk bisa store data");
            return;
        }

        if(!user.UserMetadata.IsUnityNull()){
            string username = user.UserMetadata.Username;
            var newScore = new ScoreModel{
                Player_id = username,
                Score = score,
                Playtime = playtime,
            };

            string jsonData = JsonConvert.SerializeObject(newScore);
            Debug.LogWarning($"JSON yang dikirim dari ScoreManager: {jsonData}");

            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            
            using UnityWebRequest request = new(url, "POST"){
                uploadHandler = new UploadHandlerRaw(bodyRaw),
                downloadHandler = new DownloadHandlerBuffer(),
            };


            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("apikey", SupabaseStuff.Instance.GetAPIKey());
            request.SetRequestHeader($"Authorization", "Bearer " + SupabaseStuff.Instance.GetLoggedInUserACT());

            await request.SendWebRequest();
            // request.SetRequestHeader("Authorization", $"Bearer {SupabaseStuff.Instance.GetAPIKey()}");

            if(request.result == UnityWebRequest.Result.Success) {
                Debug.Log("score berhasil disimpan");
                Debug.LogError($"Response Code: {request.responseCode}");
                Debug.LogError("Response Body: " + request.downloadHandler.text);
            } else {
                Debug.LogError($"score gagal disimpan karena: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");
                Debug.LogError("Response Body: " + request.downloadHandler.text);

            }
        } else {
            Debug.LogError("user metadata kosong gk bisa store data");
        }
    }

    // public async Task<List<ScoreModel>> FetchScores(int limit = 20)
    // {
    //     if (SupabaseStuff.Instance == null)
    //     {
    //         Debug.LogError("Supabase error: Instance is null.");
    //         return new List<ScoreModel>();
    //     }

    //     var response = await SupabaseStuff.Instance.GetSupabaseClient()
    //         .From<ScoreModel>()
    //         .Select("*")
    //         .Order("score", Constants.Ordering.Descending)
    //         .Limit(limit)
    //         .Get();

    //     if (response != null)
    //     {
    //         return response.Models.ToList();
    //     }
    //     else
    //     {
    //         Debug.Log("No scores found.");
    //         return new List<ScoreModel>();
    //     }
    // }

    public async UniTask<List<ScoreModel>> FetchScores(int limit)
    {

        string url = $"{SupabaseStuff.Instance.GetURL()}/rest/v1/scores?select=id,player_id,score,playtime&order=score.desc&limit={limit}";
        using UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SetRequestHeader("apikey", SupabaseStuff.Instance.GetAPIKey());
        webRequest.SetRequestHeader("Authorization", $"Bearer {SupabaseStuff.Instance.GetAPIKey()}");


        _ = await webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
            webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("gk dibolehin");
            Debug.LogError("Error fetching scores: " + webRequest.error);
            Debug.LogError("Response: " + webRequest.downloadHandler.text);
            return null;
        }
        else
        {
            // // Wrap the JSON array in an object to make it compatible with JsonUtility
            // string jsonResponse = $"{{\"scores\":{webRequest.downloadHandler.text}}}";
            // var wrapper = JsonUtility.FromJson<ScoreWrapper>(jsonResponse);
            // Debug.Log("makan nih list");
            // return wrapper.scores ?? new List<ScoreModel>();
            var scores = JsonConvert.DeserializeObject<List<ScoreModel>>(webRequest.downloadHandler.text);
            Debug.Log("Raw JSON Response: " + webRequest.downloadHandler.text);

            if (scores == null)
            {
                Debug.LogWarning("Deserialization returned null.");
            }
            else
            {
                Debug.Log($"Number of scores fetched: {scores.Count}");
                foreach (var score in scores)
                {
                    Debug.Log($"ID: {score.Id}, Player ID: {score.Player_id}, Score: {score.Score}, Playtime: {score.Playtime}");
                }
            }

            return scores;
        }
    }

    public void SetPlaytime(float timerPlaytime) {
        playtime = timerPlaytime;
    }
}
