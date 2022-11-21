using System;
using System.IO;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreText;

    public float highScore;
    public float score;

    int highscoreMultiplier = 5;

    string saveDataPath = "/Data/GameDataFile.json";

    public static Action<float> endGameSendScore;

    void Start()
    {
        SubscribeOutOfFuel();
        SubscribeHitObstacle();
        LoadFromJson();
    }
    void FixedUpdate()
    {
        HighscoreController();
    }
    
    void SubscribeOutOfFuel() => UIController.Instance.outOfFuel += StopIncreasingHighscore;
    void UnsubscribeOutOfFuel() => UIController.Instance.outOfFuel -= StopIncreasingHighscore;

    void SubscribeHitObstacle() => CarController.Instance.hitObstacle += StopIncreasingHighscore;
    void UnsubscribeHitObstacle() => CarController.Instance.hitObstacle -= StopIncreasingHighscore;

    void StopIncreasingHighscore()
    {
        highscoreMultiplier = 0;
        
        if (score > highScore)
            endGameSendScore?.Invoke(score);
        else
            endGameSendScore?.Invoke(highScore);
    }

    void HighscoreController()
    {
        score += highscoreMultiplier * Time.deltaTime;

        highScoreText.text = Mathf.FloorToInt(score).ToString();
    }

    void SaveToJson()
    {
        if (score > highScore)
        {
            string json = File.ReadAllText(Application.dataPath + saveDataPath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            
            data.highScore = score;
            json = JsonUtility.ToJson(data, true);
            File.WriteAllText(Application.dataPath + saveDataPath, json);
        }
    }

    void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);

        GameData data = JsonUtility.FromJson<GameData>(json);

        highScore = data.highScore;
    }

    void OnDisable()
    {
        UnsubscribeOutOfFuel();
        UnsubscribeHitObstacle();
        SaveToJson();
    }
}
