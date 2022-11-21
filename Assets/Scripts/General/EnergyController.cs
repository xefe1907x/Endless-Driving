using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    [SerializeField] int maxEnergy;
    [SerializeField] int energyRechargeDuration;
    int _energy;

    [SerializeField] TextMeshProUGUI energyText;

    const string energyKey = "Energy";
    const string energyReadyKey = "EnergyReady";

    public static Action enoughEnergy;
    
    [Space(10)] [Header("EnergyUI")]
    [SerializeField] List<GameObject> energies = new List<GameObject>();
    Queue<GameObject> energyBarQueue = new Queue<GameObject>();
    

    void Start()
    {
        SubscribePlayButtonPush();
        EnergyReadyController();
        SetupEnergyBar();
    }

    void SetupEnergyBar()
    {
        for (int i = 0; i < energies.Count; i++)
        {
            energyBarQueue.Enqueue(energies[i]);
        }
        
        if (_energy < 5)
            SetupDecreasedEnergy();
    }

    void SetupDecreasedEnergy()
    {
        for (int i = _energy; i < maxEnergy; i++)
        {
            GameObject obj = energyBarQueue.Dequeue();
            obj.SetActive(false);
        }
    }

    void SubscribePlayButtonPush() => ButtonSessions.Instance.pushedPlayButton += DateTimeController;
    void UnsubscribePlayButtonPush() => ButtonSessions.Instance.pushedPlayButton -= DateTimeController;

    void DateTimeController()
    {
        if (_energy < 1) { return; }

        _energy--;

        DecreaseEnergy();
        
        PlayerPrefs.SetInt(energyKey, _energy);

        if (_energy == 0)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(energyReadyKey,energyReady.ToString());
        }
        
        enoughEnergy?.Invoke();
    }

    void DecreaseEnergy()
    {
        GameObject obj = energyBarQueue.Dequeue();
        obj.SetActive(false);
    }

    void EnergyReadyController()
    {
        _energy = PlayerPrefs.GetInt(energyKey, maxEnergy);

        if (_energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(energyReadyKey, String.Empty);
            
            if (energyReadyString == String.Empty) { return; }
            
            DateTime energyReady = DateTime.Parse(energyReadyString);

            if (DateTime.Now > energyReady)
            {
                _energy = maxEnergy;
                PlayerPrefs.SetInt(energyKey, _energy);
            }
        }
    }

    void OnDisable()
    {
        UnsubscribePlayButtonPush();
    }
}
