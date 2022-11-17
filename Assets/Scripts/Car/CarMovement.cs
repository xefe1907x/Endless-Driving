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

    void Update()
    {
        MoveForward();
        MoveSides();
        IncreaseSpeedRegularly();
        GettingTouchInputs();
    }

    void MoveSides()
    {
        var deltaPositionX = currentPosition.x - firstPosition.x;
        
        if (Input.touchCount > 0)
        {
            transform.position =
                new Vector3(
                    Mathf.Clamp(transform.position.x + deltaPositionX * sideSpeed * Time.deltaTime, -clampValue,
                        clampValue), transform.position.y, transform.position.z);
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
            }

            if (Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                firstPosition = Vector2.zero;
                currentPosition = Vector2.zero;
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
    }
}
