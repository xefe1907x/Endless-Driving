using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    #region Singleton

    public static CarController Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    #endregion

    public event Action hitEndDetector;
    public event Action fullFuel;
    
    public event Action coinCollected;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndDetector"))
            hitEndDetector?.Invoke();
        
        if (other.GetComponent<Fuel>())
            fullFuel?.Invoke();
        
        if (other.GetComponent<Coin>())
            coinCollected?.Invoke();
            
    }
}
