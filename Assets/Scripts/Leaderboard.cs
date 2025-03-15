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
    
    async void Awake() {
        if(SupabaseStuff.Instance != null) {
            await UpdateLeaderboardUI();
        }
    }
    
    private async UniTask UpdateLeaderboardUI() {
        scoreIdx = 1;
        var topscores = await ScoreManager.Instance.FetchScores(topScoreLimit);
        foreach(var score in topscores) {
            leaderboard.text += $"{scoreIdx}.     {score.PlayerId.PadRight(15)} {score.ScoreValue}\n";
            scoreIdx++;
        }
        Debug.Log("udah selesai");
    }
}