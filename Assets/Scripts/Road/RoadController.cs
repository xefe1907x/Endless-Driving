using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    int moveThreshold = 1280;

    [SerializeField] List<GameObject> roads = new List<GameObject>();
    Queue<GameObject> roadQueue = new Queue<GameObject>();

    void Start()
    {
        CarController.Instance.hitEndDetector += MoveRoadForward;
        SetupQueue();
    }

    void SetupQueue()
    {
        for (int i = 0; i < roads.Count; i++)
        {
            roadQueue.Enqueue(roads[i]);
        }
    }

    void MoveRoadForward()
    {
        var obj = roadQueue.Peek();
        roadQueue.Dequeue();
        
        obj.SetActive(false);
        obj.transform.Translate(Vector3.forward * moveThreshold);
        obj.SetActive(true);
        roadQueue.Enqueue(obj);
    }

    void OnDisable()
    {
        CarController.Instance.hitEndDetector -= MoveRoadForward;
    }
}
