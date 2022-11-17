using System;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public int minFuel = 0;
    public int maxFuel = 5;
    int fuelIncreaseValue = 2;
    public int minObstacle = 0;
    public int maxObstacle = 30;
    int obstacleDecreaseValue = 5;

    #region Singleton

    public static DifficultyController Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    #endregion
    void Start()
    {
        CarController.Instance.hitEndDetector += IncreaseDifficulty;
    }

    void IncreaseDifficulty()
    {
        IncreaseMaxFuel();
        DecreaseObstacleValue();
    }

    void IncreaseMaxFuel() => maxFuel += fuelIncreaseValue;
    

    void DecreaseObstacleValue()
    {
        if (maxObstacle >= 10)
            maxObstacle -= obstacleDecreaseValue;
    }

    void OnDisable()
    {
        CarController.Instance.hitEndDetector -= IncreaseDifficulty;
    }
}
