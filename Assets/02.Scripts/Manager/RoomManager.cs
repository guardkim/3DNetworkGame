using System;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : SingletonPhoton<RoomManager>
{
    private Room _room;
    public Room Room => _room;

    public event Action OnRoomDataChanged;
    public event Action<string> OnPlayerEnter;
    public event Action<string> OnPlayerExit;
    public event Action<string, string> OnPlayerDied;
    protected override void Awake()
    {
        base.Awake();
    }
    // 내가 방에 입장하면 자동으로 호출되는 함수
    // 이벤트 함수의 특징은, 함수명이 상황이다.
    // 함수는 기능으로 빼야지 재사용성이 높아진다.

    private bool _initialized = false;
    protected override void Start()
    {
        Init();
    }
    public override void OnJoinedRoom()
    {
        Init();
    }
    private async void Init()
    {
        // 초기화를 한 적 있다면 return, Start가 먼저일지 OnJoinedRoom이 먼저일지 모르기 때문
        if (_initialized == true) return;
        if (!PhotonNetwork.InRoom) return;

        //OnJoinedRoom은 이미 LobbyScene에서 호출되어서(PhotonServerManager에 의해), LobbyScene에는 RoomManager가 없기 때문에 호출이 안된다.
        //따라서 OnSceneLoaded나 Start에서 따로 해줘야한다.
        // 플레이어 생성

        _initialized = true;
        // await System.Threading.Tasks.Task.Delay(100);
        GeneratePlayer();

        // 룸 설정
        SetRoom();

        OnRoomDataChanged?.Invoke();
    }
    // 새로운 플레이어가(나를 제외하고) 방에 입장하면 자동으로 호출되는 함수
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player otherPlayer)
    {
        OnRoomDataChanged?.Invoke();
        //1. 이 코드는 Manager가 UI에 대한 의존성이 생긴다.
        // UI_RoomLog.Instance.PlayerEnterLog(player.Nickname);
        //2. 두번째 방법은 UI가 MonobehaviourPunCallbacks를 상속하고, 이 함수를 override하여 사용
        //위 방식은 서버에 대한 이슈가 있을 때 UI까지 다 수정해야한다.
        //UI가 직접 서버 로직을 아는 것은 스마트 UI
        //3. 결국 관리는 Manager가... Action 하나 더 만들면 된다.
        OnPlayerEnter?.Invoke(otherPlayer.NickName + "_" + otherPlayer.ActorNumber);
    }
    // 새로운 플레이어가(나를 제외하고) 방에서 퇴장하면 자동으로 호출되는 함수
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        OnRoomDataChanged?.Invoke();
        OnPlayerExit?.Invoke(otherPlayer.NickName + "_" + otherPlayer.ActorNumber);
    }
    public void OnPlayerDeath(int actorNumber, int otherActorNumber)
    {
        // actorNumber가 otherActerNumber에 의해 죽었다.
        string deadActorNickName = _room.Players[actorNumber].NickName + "_" + actorNumber;
        string attackerActorNickName;

        // 0번은 지형에 의한 사망(DeadZone)
        if (otherActorNumber == 0)
        {
            attackerActorNickName = "지형지물";
        }
        else
        {
            attackerActorNickName = _room.Players[otherActorNumber].NickName + "_" + otherActorNumber;
        }
        OnPlayerDied?.Invoke(deadActorNickName, attackerActorNickName);
    }
    private void GeneratePlayer()
    {
        // 방에 입장 완료가 되면 플레이어를 생성한다.
        // 포톤에서는 게임 오브젝트 생성후 포톤 서버에 등록까지 해야 한다.
        Vector3 randomPosition = SpawnPoints.Instance.GetRandomSpawnPoint();

        if(PhotonServerManager.Instance.IsMale)
            PhotonNetwork.Instantiate("MalePlayer", randomPosition, Quaternion.identity).GetComponent<GameObject>();
        else
            PhotonNetwork.Instantiate("FemalePlayer", randomPosition, Quaternion.identity).GetComponent<GameObject>();
    }

    private void SetRoom()
    {
        _room = PhotonNetwork.CurrentRoom;
    }
}

