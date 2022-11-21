using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ButtonSessions : MonoBehaviour
{
    #region Singleton

    public static ButtonSessions Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    #endregion

    public event Action pushedPlayButton;

    public event Action<int, int> buyAnyButton;
    public event Action<int> openShopButton;

    [SerializeField] AudioMixer _audioMixer;

    int button2Price = 50;
    int button3Price = 500;
    int button4Price = 999;
    
    string saveDataPath = "/Data/GameDataFile.json";

    void Start()
    {
        SubscribeEnergyController();
        SubscribeWalletForBuyButton();
    }

    void SubscribeWalletForBuyButton() => WalletController.Instance.buttonBought += ButtonBought;
    void UnsubscribeWalletForBuyButton() => WalletController.Instance.buttonBought -= ButtonBought;
    
    void SubscribeEnergyController() => EnergyController.enoughEnergy += GoNextScene;
    void UnsubscribeEnergyController() => EnergyController.enoughEnergy -= GoNextScene;

    public void PlayButton()
    {
        pushedPlayButton?.Invoke();
    }

    void GoNextScene() => Invoke(nameof(LoadNextScene), 1.54f);

    public void LoadNextScene() => SceneManager.LoadScene(1);
    
    public void LoadMainMenu() => SceneManager.LoadScene(0);

    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat("volume", volume);
    }

    #region BuyButtons

    public void UseButton1()
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        data.whichCarInUse = 0;
        json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + saveDataPath, json);
    }

    public void BuyButton2()
    {
        buyAnyButton?.Invoke(button2Price, 2);
    }
    
    public void UseButton2()
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        data.whichCarInUse = 1;
        json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + saveDataPath, json);
    }

    void ButtonBought(int buttonNo)
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        if (buttonNo == 2)
            data.button2Buy = true;
        else if (buttonNo == 3)
            data.button3Buy = true;
        else if (buttonNo == 4)
            data.button4Buy = true;
        
        json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + saveDataPath, json);
        
        openShopButton?.Invoke(buttonNo);
    }
    
    public void BuyButton3()
    {
        buyAnyButton?.Invoke(button3Price, 3);
    }
    
    public void UseButton3()
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        data.whichCarInUse = 2;
        json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + saveDataPath, json);
    }
    
    public void BuyButton4()
    {
        buyAnyButton?.Invoke(button4Price, 4);
    }
    
    public void UseButton4()
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        data.whichCarInUse = 3;
        json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + saveDataPath, json);
    }

    #endregion

    void OnDisable()
    {
        UnsubscribeWalletForBuyButton();
        UnsubscribeEnergyController();
    }
}
