using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public event Action hitEndDetector;
    public event Action fullFuel;
    public event Action coinCollected;
    public event Action hitSlower;
    public event Action releasedSlower;
    public event Action hitObstacle;

    #region Singleton

    public static CarController Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    #endregion

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndDetector"))
            hitEndDetector?.Invoke();
        
        if (other.GetComponent<Fuel>())
            fullFuel?.Invoke();
        
        if (other.GetComponent<Coin>())
            coinCollected?.Invoke();
        
        if (other.CompareTag("Slower"))
            hitSlower?.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slower"))
            releasedSlower?.Invoke();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            hitObstacle?.Invoke();
        }
    }
}
