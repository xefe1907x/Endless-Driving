using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomNumberGenerator
{
    public int GenerateRandomNumber(int minNum, int maxNum)
    {
        int randomNumber = Random.Range(minNum, maxNum);

        return randomNumber;
    }
}

public class FuelSpawner : MonoBehaviour
{
    [SerializeField] List<Transform> fuelSlots = new List<Transform>();
    [SerializeField] GameObject fuelPrefab;
    
    int slotNumber;
    int minNum;
    int maxNum;

    RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();

    float spawnDelay = 0.15f;
    
    void OnEnable()
    {
        GetMinMaxValues();
        Invoke(nameof(FuelSpawn), spawnDelay);
    }

    void Start()
    {
        CreateFuels();
    }

    void GetMinMaxValues()
    {
        minNum = DifficultyController.Instance.minFuel;
        maxNum = DifficultyController.Instance.maxFuel;
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

    void SetSlotNumber() => slotNumber = _randomNumberGenerator.GenerateRandomNumber(minNum, maxNum);
    

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
