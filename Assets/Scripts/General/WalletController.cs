using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalletController : MonoBehaviour
{
    int coinAmount;
    
    string saveDataPath = "/Data/GameDataFile.json";

    int gameLevel;

    #region Singleton

    public static WalletController Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
        
        gameLevel  = SceneManager.GetActiveScene().buildIndex;
    }

    #endregion

    public event Action<int> changeCoinAmount;

    

    void Start()
    {
        GetCoinFromJson();
        if (gameLevel > 0)
            SubscribeCoinCollected();
    }
    
    void SubscribeCoinCollected() => CarController.Instance.coinCollected += CollectCoin;
    
    void UnsubscribeCoinCollected() => CarController.Instance.coinCollected -= CollectCoin;

    void GetCoinFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);

        GameData data = JsonUtility.FromJson<GameData>(json);

        coinAmount = data.coin;
        
        changeCoinAmount?.Invoke(coinAmount);
    }

    void CollectCoin()
    {
        coinAmount++;
        changeCoinAmount?.Invoke(coinAmount);
    }

    void SaveCoinsToJson()
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);
        GameData data = JsonUtility.FromJson<GameData>(json);
        
        data.coin = coinAmount;
        json = JsonUtility.ToJson(data, true);
        
        File.WriteAllText(Application.dataPath + saveDataPath, json);
    }

    void OnDisable()
    {
        if (gameLevel > 0)
            UnsubscribeCoinCollected();
        
        SaveCoinsToJson();
    }
}
