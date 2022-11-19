using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RandomItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject randomItemsParent;

    int randomSpawnNumber;
    int minNumber = 0;
    int maxNumber = 5;

    void OnEnable()
    {
        SpawnRandomItem();
    }

    void RandomSpawnNumber()
    {
        randomSpawnNumber = Random.Range(minNumber, maxNumber);
    }

    void SpawnRandomItem()
    {
        for (int i = 0; i < randomItemsParent.transform.childCount; i++)
        {
            RandomSpawnNumber();
            if (randomSpawnNumber == 0) 
                randomItemsParent.transform.GetChild(i).gameObject.SetActive(true);
            else
                randomItemsParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    void CloseAllItems()
    {
        for (int i = 0; i < randomItemsParent.transform.childCount; i++)
        {
            randomItemsParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    void OnDisable()
    {
        CloseAllItems();
    }
}
