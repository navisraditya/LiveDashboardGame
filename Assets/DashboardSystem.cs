using System.Linq;
using System.Threading.Tasks;
using App;
using TMPro;
using UnityEngine;

public class DashboardSystem : MonoBehaviour
{
    [SerializeField] TMP_Text leaderboard;
    [SerializeField] TMP_Text statsDetail;
    [SerializeField] TMP_Text topPlayerDetail;
    [SerializeField] TMP_Text yourRankDetail;
    [SerializeField] int topScoreLimit = 20;
    
    int scoreIdx = 1;
    
    async void Awake()
    {
        if(SupabaseStuff.Instance != null) {
            await UpdateLeaderboardUI();
        } else {
            SupabaseStuff.Instance.GetSupabaseClient();
            await UpdateLeaderboardUI();
        }
    }

    private async Task UpdateLeaderboardUI() {
        await UpdateLeaderboard();
        

        Debug.Log("udah selesai");
    }

    private async Task UpdateLeaderboard() {
        var user = SupabaseStuff.Instance.GetLoggedInUser();
        
        scoreIdx = 1;

        int currRank = 0;
        int playerAttempt = 0;
        float totalPlaytime = 0;
        int highestScore = 0;

        var topscores = await ScoreManager.Instance.FetchScores(topScoreLimit);

        if(topscores != null) {
            foreach(var score in topscores) {
                leaderboard.text += $"{scoreIdx}.     {score.PlayerId.PadRight(15)} {score.ScoreValue}\n";
                if(user != null) {
                    if(user.UserMetadata["username"].ToString() == score.PlayerId) {
                        currRank = scoreIdx;
                        playerAttempt++;
                        totalPlaytime += score.playtime;
                        if(highestScore >= score.ScoreValue) {
                            highestScore = score.ScoreValue;
                        }
                    }
                }
                scoreIdx++;
            }
        }

        statsDetail.text += $"Total Attempts:     {playerAttempt.ToString().PadLeft(15)} attempts\n";
        statsDetail.text += $"Highest Scores:     {highestScore.ToString().PadLeft(15)} points\n";
        statsDetail.text += $"Total Playtimes:     {totalPlaytime.ToString().PadLeft(15)} seconds\n";

        topPlayerDetail.text += $"{topscores.First().PlayerId.ToString()}";

        yourRankDetail.text += $"{currRank.ToString()}";
    }
}
