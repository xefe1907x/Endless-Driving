using System;
using System.IO;
using UnityEngine;

public class WalletController : MonoBehaviour
{
    int coinAmount;
    
    string saveDataPath = "/Data/GameDataFile.json";

    #region Singleton

    public static WalletController Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    #endregion

    public event Action<int> changeCoinAmount;

    void Start()
    {
        GetCoinFromJson();
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
        UnsubscribeCoinCollected();
        SaveCoinsToJson();
    }
}
