using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FuelSpawner : MonoBehaviour
{
    [SerializeField] List<Transform> fuelSlots = new List<Transform>();
    [SerializeField] GameObject fuelPrefab;
    
    int slotNumber;
    int minNum = 0;
    int maxNum = 5;
    
    float spawnDelay = 0.15f;
    
    void OnEnable()
    {
        CreateFuels();
        Invoke(nameof(FuelSpawn), spawnDelay);
    }

    void FuelSpawn()
    {
        foreach (Transform slot in fuelSlots)
        {
            SetSlotNumber();
            if (slotNumber == 0)
            {
                slot.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    void SetSlotNumber() => slotNumber = Random.Range(minNum, maxNum);

    void CreateFuels()
    {
        foreach (Transform slot in fuelSlots)
        {
            Instantiate(fuelPrefab, slot);
            fuelPrefab.SetActive(false);
        }
    }

    void OnDisable()
    {
        foreach (Transform slot in fuelSlots)
        {
            slot.GetChild(0).gameObject.SetActive(false);
        }
    }
}
