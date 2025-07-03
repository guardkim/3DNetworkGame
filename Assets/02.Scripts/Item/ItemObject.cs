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
            if (!player.GetComponent<PhotonView>().IsMine) return;
            if (player.State == EPlayerState.Death) return;
            switch (ItemType)
            {
                case EItemType.Score:
                    ScoreManager.Instance.AddScore(1000);
                    break;
                case EItemType.Heal:
                    //데이터와 데이터를 다루는 로직이 떨어져있다 -> 응집도가 떨어진다.
                    player.Stat.Health += Mathf.Max(player.Stat.MaxHealth, player.Stat.Health + 50);
                    break;
                case EItemType.Stamina:
                    player.Stat.Stamina += Mathf.Max(player.Stat.MaxStamina, player.Stat.Stamina + 50);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
