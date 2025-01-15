using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private int score = 0;
    public TMP_Text scoreUI;
    
    public void IncScore(){
        score++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI() {
        scoreUI.text = score.ToString();
        
    }
    
    public int GetScore(){
        return score;
    }    
}
