using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public enum EItemType
{
    Score,
    Heal,
    Stamina
}

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ItemObject : MonoBehaviourPun
{
    public EItemType ItemType;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player.State == EPlayerState.Death) return;
            switch (ItemType)
            {
                case EItemType.Score:
                    player.Score += 10;
                    break;
                case EItemType.Heal:
                    player.Stat.Health += 50;
                    break;
                case EItemType.Stamina:
                    player.Stat.Stamina += 50;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
