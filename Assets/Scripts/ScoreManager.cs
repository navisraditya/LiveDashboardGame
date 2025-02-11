using TMPro;
using UnityEngine;
using App;
using UnityEngine.SocialPlatforms.Impl;
using System.Threading.Tasks;

public class ScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text scoreUI;

    private int score = 0;
    
    public void IncScore(){
        score++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI() {
        scoreUI.text = score.ToString();
    }

    public async Task SaveScoreToSupabase() {
        if(SupabaseStuff.Instance == null) {
            return;
        }

        var user = SupabaseStuff.Instance.GetLoggedInUser();
        if(user == null) {
            Debug.Log("No Active Player. Score will not be saved");
            return;
        }

        var newScore = new ScoreModel {
            PlayerId = SupabaseStuff.Instance.GetLoggedInUser().UserMetadata["username"].ToString(),
            ScoreValue = score,
        };

    try {
        var response = await SupabaseStuff.Instance.GetSupabaseClient().From<ScoreModel>().Insert(newScore);

        if(response != null && response.Models != null) {
            Debug.Log("score saved");
        } else {
            Debug.Log("saving score failed");
        }
    }
    catch (System.Exception e) {
        Debug.LogError($"Error saving score: {e.Message}");
    }
    }
}
