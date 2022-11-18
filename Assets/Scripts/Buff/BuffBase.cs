using UnityEngine;

public class BuffBase : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
            gameObject.SetActive(false);
    }
}
