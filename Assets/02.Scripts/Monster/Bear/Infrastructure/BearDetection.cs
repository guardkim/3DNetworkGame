using UnityEngine;

public class BearDetection : MonoBehaviour
{
    private BearController _bearController;

    private void Awake()
    {
        _bearController = GetComponentInParent<BearController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _bearController.Player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _bearController.Player = null;
        }
    }
}
