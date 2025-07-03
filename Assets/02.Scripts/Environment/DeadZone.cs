using System;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null)
            {
                photonView.RPC("Damaged", RpcTarget.All, 100000000f, 0);
            }
            else
            {
                // PhotonView가 없는 경우 직접 데미지 적용 (예: 로컬 플레이어)
                damageable.Damaged(100000000, 0);
            }
        }
    }
}
