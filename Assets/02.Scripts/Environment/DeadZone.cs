using System;
using Unity.VisualScripting;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<Player>().Damaged(100000000, 0);
        }
    }
}
