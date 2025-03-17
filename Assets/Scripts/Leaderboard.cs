using App;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] TMP_Text leaderboard;
    [SerializeField] TMP_Text playerPosition;
    [SerializeField] int topScoreLimit = 20;
    
    int scoreIdx = 1;
    
    async void Start() {
        if(SupabaseStuff.Instance != null) {
            await UpdateLeaderboardUI();
        }
    }
    
    private async UniTask UpdateLeaderboardUI() {
        scoreIdx = 1;
        var topscores = await ScoreManager.Instance.FetchScores(topScoreLimit);
        foreach(var score in topscores) {
            leaderboard.text += $"{scoreIdx}.     {score.Player_id,-15} {score.Score}\n";
            scoreIdx++;
        }
        Debug.Log("udah selesai");
    }
}