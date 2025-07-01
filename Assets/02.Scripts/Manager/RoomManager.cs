using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomManager : SingletonPhoton<RoomManager>
{
    private Room _room;
    public Room Room => _room;

    public event Action OnRoomDataChanged;
    protected override void Awake()
    {
        base.Awake();
    }
    // 내가 방에 입장하면 자동으로 호출되는 함수
    // 이벤트 함수의 특징은, 함수명이 상황이다.
    // 함수는 기능으로 빼야지 재사용성이 높아진다.
    public override void OnJoinedRoom()
    {
        // 플레이어 생성
        GeneratePlayer();

        // 룸 설정
        SetRoom();

        OnRoomDataChanged?.Invoke();
    }
    // 새로운 플레이어가(나를 제외하고) 방에 입장하면 자동으로 호출되는 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
    {
        OnRoomDataChanged?.Invoke();
    }
    // 새로운 플레이어가(나를 제외하고) 방에서 퇴장하면 자동으로 호출되는 함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
    {
        OnRoomDataChanged?.Invoke();
    }
    private void GeneratePlayer()
    {
        // 방에 입장 완료가 되면 플레이어를 생성한다.
        // 포톤에서는 게임 오브젝트 생성후 포톤 서버에 등록까지 해야 한다.
        Vector3 randomPosition = SpawnPoints.Instance.GetRandomSpawnPoint();
        Player _player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<Player>();
        _player.transform.position = randomPosition;
    }

    private void SetRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
        Debug.Log(_room.Name);
        Debug.Log(_room.PlayerCount);
        Debug.Log(_room.MaxPlayers);
    }
}
