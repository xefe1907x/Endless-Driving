using System;
using System.Collections;
using UnityEngine;
using TouchPhase = UnityEngine.TouchPhase;

public class CarMovement : MonoBehaviour
{
    [Header("MoveForward")]
    [SerializeField] float carSpeed = 10f;
    [SerializeField] float maxSpeed = 50f;
    [SerializeField] float carAcceleration = 10f;
    [Space(5)]
    
    [Header("MoveSides")]
    [SerializeField] [Range(0.001f, 0.1f)] float sideSpeed;
    [SerializeField] float clampValue = 6f;

    Vector2 firstPosition;
    Vector2 currentPosition;

    float deltaPositionX;

    bool outOfFuel;

    #region Singleton

    public static CarMovement Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    #endregion

    public event Action<float> xPosition;
    public event Action touchCanceled;
    public event Action<float> carSpeedAction;

    void Start()
    {
        SubscribeOutOfFuel();
    }

    void Update()
    {
        MoveForward();
        MoveSides();
        IncreaseSpeedRegularly();
        GettingTouchInputs();
    }
    
    void SubscribeOutOfFuel() => UIController.Instance.outOfFuel += StopCar;
    void UnsubscribeOutOfFuel() => UIController.Instance.outOfFuel -= StopCar;

    void MoveSides()
    {
        if (Input.touchCount > 0)
        {
            transform.position =
                new Vector3(
                    Mathf.Clamp(transform.position.x + deltaPositionX * sideSpeed * Time.deltaTime, -clampValue,
                        clampValue), transform.position.y, transform.position.z);
            
            xPosition?.Invoke(deltaPositionX);
        }
    }

    void GettingTouchInputs()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstPosition = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                currentPosition = Input.GetTouch(0).position;
                
                deltaPositionX = currentPosition.x - firstPosition.x;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                deltaPositionX = 0;
                
                touchCanceled?.Invoke();
            }
        }
    }

    void IncreaseSpeedRegularly()
    {
        if (carSpeed <= maxSpeed)
            carSpeed += carAcceleration * Time.deltaTime;
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);
        
        carSpeedAction?.Invoke(carSpeed);
    }

    void StopCar()
    {
        StartCoroutine(nameof(CarSlowDown));
        sideSpeed = 0;
        maxSpeed = -1;
        carAcceleration = 0;
    }

    IEnumerator CarSlowDown()
    {
        if (carSpeed > 0)
        {
            carSpeed -= 0.03f;

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(nameof(CarSlowDown));
        }
        
        if (carSpeed < 0)
            carSpeed = 0;
    }

    void OnDisable()
    {
        UnsubscribeOutOfFuel();
    }
}
