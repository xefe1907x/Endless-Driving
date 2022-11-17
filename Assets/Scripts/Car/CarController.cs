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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndDetector"))
            hitEndDetector?.Invoke();
    }
}
