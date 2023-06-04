using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] TextMeshProUGUI scoreText;

    private int score;

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }


    private void UpdateScoreText()
    {
        scoreText.text = string.Format("Score: {0}", score);
    }

    public async void SendHighScore(int newScore)
    {
        bool state = await DatabaseManager.Instance.UpdateScore(newScore);
        if (state)
        {
            Debug.Log("Completed");
        }
        else
        {
            Debug.Log("Your new score lower then your highscore!");
        }
    }

    private void Start()
    {
        UpdateScoreText();
    }

    public int GetScore()
    {
        return score;
    }
}
