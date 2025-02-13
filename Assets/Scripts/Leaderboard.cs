using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using App;
using NUnit.Framework;
using TMPro;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] TMP_Text leaderboard;
    [SerializeField] TMP_Text playerPosition;
    [SerializeField] int topScoreLimit = 20;
    async void Awake() {        
        if(SupabaseStuff.Instance != null) {
            await UpdateLeaderboardUI();
        }
    }
    
    private async Task UpdateLeaderboardUI() {
        var topscores = await ScoreManager.Instance.FetchScores(topScoreLimit);
        foreach(var score in topscores) {
            for(int i = 0; i <= topScoreLimit; i++) {
                leaderboard.text = $"{i}. {score.PlayerId} {score.ScoreValue}";
            }
        }
        Debug.Log("udah selesai");
    }
}