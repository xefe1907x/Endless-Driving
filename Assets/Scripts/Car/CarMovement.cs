using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("MoveForward")]
    [SerializeField] float carSpeed = 10f;
    [SerializeField] float carAcceleration = 0.2f;
    [Space(5)]
    
    [Header("Steer")]
    [SerializeField] float steerValue = 10f;
    void Update()
    {
        MoveForward();
        IncreaseSpeedRegularly();
        SteerCar();
    }

    void SteerCar()
    {
        // transform.Rotate(0f, );
    }

    void IncreaseSpeedRegularly()
    {
        carSpeed += carAcceleration * Time.deltaTime;
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);
    }

    public void Steer(int value)
    {
        steerValue = value;
    }
}
