using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSessions : MonoBehaviour
{
    #region Singleton

    public static ButtonSessions Instance;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    #endregion

    public event Action pushedPlayButton;

    void Start()
    {
        SubscribeEnergyController();
    }
    
    void SubscribeEnergyController() => EnergyController.enoughEnergy += LoadNextScene;
    void UnsubscribeEnergyController() => EnergyController.enoughEnergy -= LoadNextScene;

    public void PlayButton()
    {
        pushedPlayButton?.Invoke();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    void OnDisable()
    {
        UnsubscribeEnergyController();
    }
}
