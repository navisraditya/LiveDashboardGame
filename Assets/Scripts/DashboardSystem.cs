// using System.Linq;
// using App;
// using Cysharp.Threading.Tasks;
// using TMPro;
// using UnityEngine;

// public class DashboardSystem : MonoBehaviour
// {
//     public static DashboardSystem Instance {get; set;}

//     [SerializeField] TMP_Text leaderboard;
//     [SerializeField] TMP_Text statsDetail;
//     [SerializeField] TMP_Text topPlayerDetail;
//     [SerializeField] TMP_Text yourRankDetail;
//     [SerializeField] int topScoreLimit = 20;
//     [SerializeField] CanvasGroup loading;
//     [SerializeField] CanvasGroup loginBanner;


//     int scoreIdx = 1;

//     async void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject); // Ensure only one instance exists
//             return;
//         }

//         if(SupabaseStuff.Instance != null) {
//             await UpdateLeaderboardUI();
//         } else {
//             SupabaseStuff.Instance.GetSupabaseClient();
//             await UpdateLeaderboardUI();
//         }
//     }

//     private async UniTask UpdateLeaderboardUI() {
//         loading.gameObject.SetActive(true);
//         await UpdateLeaderboard();


//         Debug.Log("udah selesai");
//         loading.gameObject.SetActive(false);
//     }

//     private async UniTask UpdateLeaderboard() {
//         var user = SupabaseStuff.Instance.GetLoggedInUser();

//         scoreIdx = 1;

//         int? currRank = null;
//         int playerAttempt = 0;
//         float totalPlaytime = 0;
//         int highestScore = 0;

//         var topscores = await ScoreManager.Instance.FetchScores(topScoreLimit);

//         if(topscores != null) {
//             foreach(var score in topscores) {
//                 leaderboard.text += $"{scoreIdx}.     {score.PlayerId.PadRight(15)} {score.ScoreValue}\n";
//                 if(user != null) {
//                     if(user.UserMetadata["username"].ToString() == score.PlayerId) {
//                         if(currRank == null) {
//                             currRank = scoreIdx;
//                         }
//                         playerAttempt++;
//                         totalPlaytime += score.playtime;
//                         if(highestScore >= score.ScoreValue) {
//                             highestScore = score.ScoreValue;
//                         }
//                     }
//                 }
//                 scoreIdx++;
//             }
//         }

//         if(user == null) {
//             loginBanner.gameObject.SetActive(true);
//             currRank = null;
//         } else {
//             loginBanner.gameObject.SetActive(false);
//             statsDetail.text += $"Total Attempts:     {playerAttempt.ToString().PadLeft(15)} attempts\n";
//             statsDetail.text += $"Highest Scores:     {highestScore.ToString().PadLeft(15)} points\n";
//             statsDetail.text += $"Total Playtimes:     {totalPlaytime.ToString().PadLeft(15)} seconds\n";
//         }

//         topPlayerDetail.text += $"{topscores.First().PlayerId.ToString()}";

//         yourRankDetail.text += $"{currRank.ToString()}";
//     }
// }


using System.Collections.Generic;
using System.Linq;
using App;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DashboardSystem : MonoBehaviour
{
    public static DashboardSystem Instance { get; set; }

    [SerializeField] private TMP_Text leaderboard;
    [SerializeField] private TMP_Text statsDetail;
    [SerializeField] private TMP_Text topPlayerDetail;
    [SerializeField] private TMP_Text yourRankDetail;
    [SerializeField] private int topScoreLimit = 20;
    [SerializeField] private CanvasGroup loading;
    [SerializeField] private CanvasGroup loginBanner;

    private const string SUPABASE_URL = "https://rbmxqlqzyemtwsajfjtw.supabase.co";
    private const string SUPABASE_API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJibXhxbHF6eWVtdHdzYWpmanR3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzcwMDgyNjcsImV4cCI6MjA1MjU4NDI2N30.N2-ULM2_1zc_yCo3zoYlolIZhX8OPnixsILHqhZxTO8";

    private int scoreIdx = 1;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        await UpdateLeaderboardUI();
    }

    // private async UniTask UpdateLeaderboardUI()
    // {
    //     loading.gameObject.SetActive(true);
    //     await UpdateLeaderboard();
    //     Debug.Log("udah selesai");
    //     loading.gameObject.SetActive(false);
    // }

    // private async UniTask UpdateLeaderboard()
    // {
    //     // Fetch scores directly from Supabase (public access)
    //     var topscores = await FetchScores(topScoreLimit);

    //     if (topscores != null)
    //     {
    //         leaderboard.text = ""; // Clear the leaderboard text before updating
    //         scoreIdx = 1;

    //         foreach (var score in topscores)
    //         {
    //             leaderboard.text += $"{scoreIdx}.     {score.player_id.PadRight(15)} {score.score}\n";
    //             scoreIdx++;
    //         }
    //     }

    //     // Check if a user is logged in
    //     var user = SupabaseStuff.Instance?.GetLoggedInUser();

    //     if (user != null)
    //     {
    //         // User is logged in
    //         loginBanner.gameObject.SetActive(false);

    //         // Calculate user stats
    //         int playerAttempt = 0;
    //         float totalPlaytime = 0;
    //         int highestScore = 0;
    //         int? currRank = null;

    //         if (topscores != null)
    //         {
    //             foreach (var score in topscores)
    //             {
    //                 if (user.UserMetadata["username"].ToString() == score.player_id)
    //                 {
    //                     if (currRank == null)
    //                     {
    //                         currRank = scoreIdx;
    //                     }
    //                     playerAttempt++;
    //                     totalPlaytime += score.playtime;
    //                     if (score.score > highestScore)
    //                     {
    //                         highestScore = score.score;
    //                     }
    //                 }
    //             }
    //         }

    //         // Update stats detail
    //         statsDetail.text = $"Total Attempts:     {playerAttempt.ToString().PadLeft(15)} attempts\n";
    //         statsDetail.text += $"Highest Scores:     {highestScore.ToString().PadLeft(15)} points\n";
    //         statsDetail.text += $"Total Playtimes:     {totalPlaytime.ToString().PadLeft(15)} seconds\n";

    //         // Update top player detail
    //         topPlayerDetail.text = topscores != null && topscores.Any() ? topscores.First().player_id : "N/A";

    //         // Update your rank detail
    //         yourRankDetail.text = currRank.HasValue ? currRank.Value.ToString() : "N/A";
    //     }
    //     else
    //     {
    //         // No user is logged in
    //         loginBanner.gameObject.SetActive(true);
    //         statsDetail.text = "";
    //         topPlayerDetail.text = topscores != null && topscores.Any() ? topscores.First().player_id : "N/A";
    //         yourRankDetail.text = "N/A";
    //     }
    // }

private async UniTask UpdateLeaderboardUI()
{
    loading.gameObject.SetActive(true);

    // Ensure Supabase client is initialized
    if (SupabaseStuff.Instance == null || SupabaseStuff.Instance.GetSupabaseClient() == null)
    {
        Debug.LogError("Supabase client is not initialized.");
        loading.gameObject.SetActive(false);
        return;
    }

    await UpdateLeaderboard();
    Debug.Log("Leaderboard update complete.");
    loading.gameObject.SetActive(false);
}

private async UniTask UpdateLeaderboard()
{
    var user = SupabaseStuff.Instance.GetLoggedInUser();

    if (user == null)
    {
        Debug.Log("No user is logged in. Displaying public leaderboard.");
        loginBanner.gameObject.SetActive(true);
        statsDetail.text = "";
        topPlayerDetail.text = "N/A";
        yourRankDetail.text = "N/A";
    }
    else
    {
        Debug.Log($"User is logged in: {user.Email}");
        loginBanner.gameObject.SetActive(false);

        // Fetch and display user-specific data
        // ...
    }

    // Fetch and display the leaderboard
    var topscores = await FetchScores(topScoreLimit);
    if (topscores != null)
    {
        leaderboard.text = ""; // Clear the leaderboard text before updating
        scoreIdx = 1;

        foreach (var score in topscores)
        {
            leaderboard.text += $"{scoreIdx}.     {score.player_id.PadRight(15)} {score.score}\n";
            scoreIdx++;
        }
    }
}
    private async UniTask<List<Score>> FetchScores(int limit)
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
                Debug.LogError("Response: " + webRequest.downloadHandler.text);
                return null;
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

    [System.Serializable]
    public class Score
    {
        public int id; // Unique identifier for the score
        public string player_id; // Player ID or username
        public int score; // Player's score
        public float playtime; // Total playtime in seconds
    }

    [System.Serializable]
    private class ScoreWrapper
    {
        public List<Score> scores;
    }
}