using System.Threading.Tasks;
using App;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] TMP_Text leaderboard;
    [SerializeField] TMP_Text playerPosition;
    [SerializeField] int topScoreLimit = 20;
    
    int scoreIdx = 1;
    
    async void Awake() {
        if(SupabaseStuff.Instance != null) {
            await UpdateLeaderboardUI();
        }
    }
    
    private async Task UpdateLeaderboardUI() {
        scoreIdx = 1;
        var topscores = await ScoreManager.Instance.FetchScores(topScoreLimit);
        foreach(var score in topscores) {
            leaderboard.text += $"{scoreIdx}.     {score.player_id,-15} {score.score}\n";
            scoreIdx++;
        }
        Debug.Log("udah selesai");
    }
}