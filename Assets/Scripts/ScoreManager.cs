using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using Postgrest;
using System.Linq;
using System;
using App;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TMP_Text scoreUI;

    private int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    public async Task SaveScoreToSupabase()
    {
        if (SupabaseStuff.Instance == null)
        {
            Debug.LogError("Supabase error: Instance is null.");
            return;
        }

        var user = SupabaseStuff.Instance.GetLoggedInUser();
        if (user == null)
        {
            Debug.Log("No Active Player. Score will not be saved.");
            return;
        }

        var newScore = new ScoreModel
        {
            PlayerId = user.UserMetadata["username"].ToString(),
            ScoreValue = score,
        };

        try
        {
            var response = await SupabaseStuff.Instance.GetSupabaseClient()
                .From<ScoreModel>()
                .Insert(newScore);

            if (response != null && response.Models != null)
            {
                Debug.Log("Score saved.");
            }
            else
            {
                Debug.Log("Saving score failed.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving score: {e.Message}");
        }
    }

    public async Task<List<ScoreModel>> FetchScores(int limit = 20)
    {
        if (SupabaseStuff.Instance == null)
        {
            Debug.LogError("Supabase error: Instance is null.");
            return new List<ScoreModel>();
        }

        var response = await SupabaseStuff.Instance.GetSupabaseClient()
            .From<ScoreModel>()
            .Select("*")
            .Order("score", Constants.Ordering.Descending)
            .Limit(limit)
            .Get();

        if (response != null)
        {
            return response.Models.ToList();
        }
        else
        {
            Debug.Log("No scores found.");
            return new List<ScoreModel>();
        }
    }
}
