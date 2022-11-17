using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] List<Transform> obstacleSlots = new List<Transform>();
    [SerializeField] GameObject[] obstacles;

    int slotNumber;
    int minNum = 0;
    int maxNum = 30;
    
    float spawnDelay = 0.15f;

    void OnEnable()
    {
        CreateObstacles();
        Invoke(nameof(BlockSpawner), spawnDelay);
    }

    void BlockSpawner()
    {
        foreach (Transform slot in obstacleSlots)
        {
            SetSlotNumber();
            if (slotNumber == 0)
            {
                slot.GetChild(0).gameObject.SetActive(true);
            }
        
            else if (slotNumber == 1)
            {
                slot.GetChild(1).gameObject.SetActive(true);
            }
        
            else if (slotNumber == 2)
            {
                slot.GetChild(2).gameObject.SetActive(true);
            }
        
            else if (slotNumber == 3)
            {
                slot.GetChild(3).gameObject.SetActive(true);
            }
        }
    }

    void SetSlotNumber() => slotNumber = Random.Range(minNum, maxNum);

    void CreateObstacles()
    {
        foreach (Transform slot in obstacleSlots)
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                Instantiate(obstacles[i], slot);
                obstacles[i].SetActive(false);
            }
        }
    }
    
    void OnDisable()
    {
        foreach (Transform slot in obstacleSlots)
        {
            for (int i = 0; i < obstacles.Length; i++)
                obstacles[i].SetActive(false);
            
        }
    }
}
