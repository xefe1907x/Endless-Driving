using System;
using System.IO;
using TMPro;
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
    public event Action<int> buttonBought;

    [SerializeField] TextMeshProUGUI walletText;
    

    void Start()
    {
        GetCoinFromJson();
        SubscribeBuyButtons();
        SetWalletText();
        
        if (gameLevel > 0)
            SubscribeCoinCollected();
    }

    void SetWalletText()
    {
        walletText.text = coinAmount.ToString();
    }

    void BuyButtonIsPressed(int price, int buttonNo)
    {
        if (coinAmount >= price)
        {
            buttonBought?.Invoke(buttonNo);

            coinAmount -= price;
            
            string json = File.ReadAllText(Application.dataPath + saveDataPath);
            GameData data = JsonUtility.FromJson<GameData>(json);

            data.coin = coinAmount;
            json = JsonUtility.ToJson(data, true);
            File.WriteAllText(Application.dataPath + saveDataPath, json);

            SetWalletText();
        }
    }
    
    void SubscribeBuyButtons() => ButtonSessions.Instance.buyAnyButton += BuyButtonIsPressed;
    void UnsubscribeBuyButtons() => ButtonSessions.Instance.buyAnyButton -= BuyButtonIsPressed;
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
        UnsubscribeBuyButtons();
        
        if (gameLevel > 0)
            UnsubscribeCoinCollected();
        
        SaveCoinsToJson();
    }
}
