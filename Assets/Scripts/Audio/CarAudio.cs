using UnityEngine;

public class CarAudio : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip carConstant;
    [SerializeField] AudioClip carStart;
    [SerializeField] AudioClip coinCollect;
    [SerializeField] AudioClip fuelCollect;
    

    void Start()
    {
        SetupAudioController();
        StartCarSound();
        CarGoesConstant();
        SubscribeForSounds();
    }

    void SubscribeForSounds()
    {
        CarController.Instance.coinCollected += CoinCollectSound;
        CarController.Instance.fullFuel += FuelCollectSound;
        CarController.Instance.hitObstacle += StopSounds;
    }

    void StopSounds() => CancelInvoke();
    
    void UnsubscribeForSounds()
    {
        CarController.Instance.coinCollected -= CoinCollectSound;
        CarController.Instance.fullFuel -= FuelCollectSound;
        CarController.Instance.hitObstacle -= StopSounds;
    }
    void CoinCollectSound() => audioSource.PlayOneShot(coinCollect);
    void FuelCollectSound() => audioSource.PlayOneShot(fuelCollect);

    void StartCarSound() => audioSource.PlayOneShot(carStart);

    void CarGoesConstant() => InvokeRepeating(nameof(StartConstantCarSound), 3.1f, 0.25f);

    void StartConstantCarSound() => audioSource.PlayOneShot(carConstant);
    

    void SetupAudioController() => audioSource = GetComponent<AudioSource>();

    void OnDisable()
    {
        UnsubscribeForSounds();
    }
}
