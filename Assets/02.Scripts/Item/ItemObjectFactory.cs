using Photon.Pun;
using UnityEngine;


[RequireComponent(typeof(PhotonView))]
public class ItemObjectFactory : MonoBehaviourPun
{
    public static ItemObjectFactory Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void RequestCreate(EItemType itemType, Vector3 dropPosition)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Create(itemType, dropPosition);
        }
        else
        {
            //MasterClient는 마스터에게만 호출하는 것
            photonView.RPC(nameof(Create), RpcTarget.MasterClient, itemType, dropPosition);
        }
    }

    [PunRPC]
    private void Create(EItemType itemType, Vector3 dropPosition)
    {
        PhotonNetwork.InstantiateRoomObject($"{itemType}Item", dropPosition + new Vector3(0,2,0),  Quaternion.identity, 0);
    }

    public void RequestDelete(int viewId)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Delete(viewId);
        }
        else
        {
            photonView.RPC(nameof(Delete), RpcTarget.MasterClient, viewId);
        }
    }

    [PunRPC]
    private void Delete(int viewId)
    {
        GameObject objectToDelete = PhotonView.Find(viewId).gameObject;
        if (objectToDelete == null) return;
        PhotonNetwork.Destroy(objectToDelete);
    }
}
