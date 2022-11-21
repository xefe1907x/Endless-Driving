using System;
using System.IO;
using UnityEngine;
public class CarSpawner : MonoBehaviour
{
    [SerializeField] GameObject targetParent;

    string saveDataPath = "/Data/GameDataFile.json";

    int whichCarInUse;

    void Start()
    {
        CarSelector();
        CarInstantiator();
    }

    void CarInstantiator()
    {
        switch (whichCarInUse)
        {
            case 0:
                targetParent.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 1:
                targetParent.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 2:
                targetParent.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 3:
                targetParent.transform.GetChild(3).gameObject.SetActive(true);
                break;
        }
    }

    void CarSelector()
    {
        string json = File.ReadAllText(Application.dataPath + saveDataPath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        whichCarInUse = data.whichCarInUse;
    }
}
