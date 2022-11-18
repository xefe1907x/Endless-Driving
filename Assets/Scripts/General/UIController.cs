using System;
using DG.Tweening;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UIController : MonoBehaviour
{
    [Header("Panel Objects")]
    [SerializeField] GameObject steeringWheel;
    [SerializeField] Image fuelFull;
    [SerializeField] GameObject speedBar;
    [Space(10)]

    [Header("Values")]
    [SerializeField] float rotateValueDivider = 5f;
    [SerializeField] float fuelDecreaseDivider = 10000f;
    [SerializeField] float speedBarMultiplier;

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

    void Start()
    {
        CarMovement.Instance.xPosition += MoveSteeringWheel;
        CarMovement.Instance.touchCanceled += StopMovingSteer;
        CarMovement.Instance.carSpeedAction += DecreaseFuel;
        CarMovement.Instance.carSpeedAction += ManageSpeedBar;
        CarController.Instance.fullFuel += FullFuel;
        DOTween.Init();
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
    

    void OnDisable()
    {
        CarMovement.Instance.xPosition -= MoveSteeringWheel;
        CarMovement.Instance.touchCanceled -= StopMovingSteer;
        CarMovement.Instance.carSpeedAction -= DecreaseFuel;
        CarMovement.Instance.carSpeedAction -= ManageSpeedBar;
        CarController.Instance.fullFuel -= FullFuel;
    }
}
