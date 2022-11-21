using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip buyButton;
    [SerializeField] AudioClip playButton;
    [SerializeField] AudioClip hitObstacle;

    int gameLevel;

    void Start()
    {
        GetLevel();
        SetupAudioController();

        if (gameLevel < 1)
        {
            SubscribePlayButton();
            SubscribePurschaseButton();
        }

        if (gameLevel > 0)
            SubscribeHitObstacleSound();
    }
    void GetLevel() => gameLevel = SceneManager.GetActiveScene().buildIndex;
    
    void SubscribePurschaseButton() => ButtonSessions.Instance.openShopButton += ClickBuyButtonSound;
    void UnsubscribePurschaseButton() => ButtonSessions.Instance.openShopButton -= ClickBuyButtonSound;
    void SubscribeHitObstacleSound() => CarController.Instance.hitObstacle += HitObstacleSound;
    void UnsubscribeHitObstacleSound() => CarController.Instance.hitObstacle -= HitObstacleSound;
    
    void SubscribePlayButton() => EnergyController.enoughEnergy += ClickPlayButtonSound;
    void UnsubscribePlayButton() => EnergyController.enoughEnergy -= ClickPlayButtonSound;

    void SetupAudioController() => audioSource = GetComponent<AudioSource>();

    public void HitObstacleSound() => audioSource.PlayOneShot(hitObstacle);
    public void ClickButtonSound() => audioSource.PlayOneShot(buttonClick);
    public void ClickBuyButtonSound(int buff) => audioSource.PlayOneShot(buyButton);
    public void ClickPlayButtonSound() => audioSource.PlayOneShot(playButton);

    void OnDisable()
    {
        if (gameLevel < 1)
        {
            UnsubscribePlayButton();
            UnsubscribePurschaseButton();
        }
        
        if (gameLevel > 0)
            UnsubscribeHitObstacleSound();
    }
}
