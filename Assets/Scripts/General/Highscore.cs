using System;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;

    float score;
    float currentHighscore;

    int highscoreMultiplier = 5;
    
    string highScoreKey = "highScore";

    void Start()
    {
        UIController.Instance.outOfFuel += StopIncreasingHighscore;
    }

    void FixedUpdate()
    {
        HighscoreController();
    }

    void HighscoreController()
    {
        score += highscoreMultiplier * Time.deltaTime;

        highScoreText.text = Mathf.FloorToInt(score).ToString();
    }

    void StopIncreasingHighscore() => highscoreMultiplier = 0;


    void OnDisable()
    {
        UIController.Instance.outOfFuel -= StopIncreasingHighscore;
    }

    void OnDestroy()
    {
        currentHighscore = PlayerPrefs.GetFloat(highScoreKey);
        
        if (currentHighscore < score)
            PlayerPrefs.SetFloat(highScoreKey, score);
    }
}
