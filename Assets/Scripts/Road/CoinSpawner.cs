using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] List<Transform> coinSlots = new List<Transform>();
    [SerializeField] GameObject coinPrefab;

    int slotNumber;
    int minNum = 0;
    int maxNum = 4;
    
    float spawnDelay = 0.15f;
    
    void OnEnable()
    {
        Invoke(nameof(CoinSpawn), spawnDelay);
    }

    void Start()
    {
        CreateCoins();
    }

    void CreateCoins()
    {
        foreach (Transform slot in coinSlots)
        {
            Instantiate(coinPrefab, slot);
            coinPrefab.SetActive(false);
        }
    }
    void CoinSpawn()
    {
        foreach (Transform slot in coinSlots)
        {
            SetSlotNumber();
            if (slotNumber == 0)
            {
                slot.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    void SetSlotNumber() => slotNumber = Random.Range(minNum, maxNum);

    

    void OnDisable()
    {
        foreach (Transform slot in coinSlots)
        {
            slot.GetChild(0).gameObject.SetActive(false);
        }
    }
}
