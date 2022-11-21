using System;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class UIController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] GameObject steeringWheel;
    [SerializeField] Image fuelFull;
    [SerializeField] GameObject speedBar;
    [SerializeField] TextMeshProUGUI coin;
    [Space(10)]

    [Header("Values")]
    [SerializeField] float rotateValueDivider = 5f;
    [SerializeField] float fuelDecreaseDivider = 10000f;
    [SerializeField] float speedBarMultiplier;
    [Space(10)]

    [Header("GameOver UI")]
    [SerializeField] TextMeshProUGUI highscoreText;

    [SerializeField] GameObject losePanel;
    float activateDelay = 3.5f;
    
    [Space(10)]

    [Header("Store")]
    [SerializeField] GameObject boughtAfterBuyButton1;
    [SerializeField] GameObject boughtAfterBuyButton2;
    [SerializeField] GameObject boughtAfterBuyButton3;
    
    string saveDataPath = "/Data/GameDataFile.json";
    

    #region Singleton

    public static UIController Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    #endregion

    public event Action outOfFuel;

    int gameLevel;

    void OnEnable()
    {
        SetGameLevel();
        if (gameLevel > 0)
            SubscribeActions();
        DOTween.Init();
        if (gameLevel < 1)
            BuyButtonController();
    }

    void Start()
    {
        if (gameLevel < 1)
            SubscribeOpenShopButton();
    }

    void SetGameLevel() => gameLevel = SceneManager.GetActiveScene().buildIndex;

    void BuyButtonController()
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        var buyButton2 = data.button2Buy;
        var buyButton3 = data.button3Buy;
        var buyButton4 = data.button4Buy;
        
        if (buyButton2)
            boughtAfterBuyButton1.SetActive(true);
        if (buyButton3)
            boughtAfterBuyButton2.SetActive(true);
        if (buyButton4)
            boughtAfterBuyButton3.SetActive(true);
    }

    void ShopButtonOpener(int buttonNo)
    {
        switch (buttonNo)
        {
            case 2:
                boughtAfterBuyButton1.SetActive(true);
                break;
            case 3:
                boughtAfterBuyButton2.SetActive(true);
                break;
            case 4:
                boughtAfterBuyButton3.SetActive(true);
                break;
        }
    }

    void SubscribeOpenShopButton() => ButtonSessions.Instance.openShopButton += ShopButtonOpener;
    void UnsubscribeOpenShopButton() => ButtonSessions.Instance.openShopButton -= ShopButtonOpener;
    

    void SubscribeActions()
    {
        CarMovement.Instance.xPosition += MoveSteeringWheel;
        CarMovement.Instance.touchCanceled += StopMovingSteer;
        CarMovement.Instance.carSpeedAction += DecreaseFuel;
        CarMovement.Instance.carSpeedAction += ManageSpeedBar;
        CarController.Instance.fullFuel += FullFuel;
        WalletController.Instance.changeCoinAmount += ChangeWalletAmount;
        Highscore.endGameSendScore += SetHighscoreToUI;
    }

    void SetHighscoreToUI(float bestScore)
    {
        highscoreText.text = Mathf.FloorToInt(bestScore).ToString();
        Invoke(nameof(ActivateLosePanel), activateDelay);
    }

    void ActivateLosePanel()
    {
        losePanel.SetActive(true);
    }

    void ChangeWalletAmount(int coinAmount)
    {
        coin.text = coinAmount.ToString();
    }

    void MoveSteeringWheel(float value)
    {
        Vector3 rotateValue = new Vector3(0, 0, -value/rotateValueDivider);
        
        steeringWheel.transform.DOLocalRotate(rotateValue, 0.5f);
    }

    void StopMovingSteer()
    {
        Vector3 endValue = new Vector3(0, 0, 0);
        
        steeringWheel.transform.DOLocalRotate(endValue, 0.5f);
    }

    void DecreaseFuel(float decreaseValue)
    {
        fuelFull.fillAmount -= decreaseValue/fuelDecreaseDivider;
        
        if (fuelFull.fillAmount <= 0.01)
            outOfFuel?.Invoke();
    }

    void ManageSpeedBar(float speed)
    {
        Vector3 value = new Vector3(0f, 0f, -speed * speedBarMultiplier);

        speedBar.transform.DOLocalRotate(value, 0.5f);
    }

    void FullFuel() => fuelFull.fillAmount = 1;

    void UnsubscribeActions()
    {
        CarMovement.Instance.xPosition -= MoveSteeringWheel;
        CarMovement.Instance.touchCanceled -= StopMovingSteer;
        CarMovement.Instance.carSpeedAction -= DecreaseFuel;
        CarMovement.Instance.carSpeedAction -= ManageSpeedBar;
        CarController.Instance.fullFuel -= FullFuel;
        WalletController.Instance.changeCoinAmount -= ChangeWalletAmount;
        Highscore.endGameSendScore -= SetHighscoreToUI;
    }
    

    void OnDisable()
    {
        if (gameLevel < 1)
            UnsubscribeOpenShopButton();
        if (gameLevel > 0)
            UnsubscribeActions();
    }
}
