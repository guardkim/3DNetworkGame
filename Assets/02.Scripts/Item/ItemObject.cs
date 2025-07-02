using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.Score += 10;

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
