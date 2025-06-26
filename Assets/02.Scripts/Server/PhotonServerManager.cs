using System.Collections.Generic;
using UnityEngine;

// Photon API 네임스페이스
using Photon.Pun;
using Photon.Realtime;

// 역할 : 포톤 서버 관리자(서버 연결, 로비 입장, 방 입장, 게임 입장)
public class PhotonServerManager : MonoBehaviourPunCallbacks
{
    // MonoBehaviourPunCallbacks : 유니티 이벤트 말고도 PUN 서버 이벤트를 받을 수 있다.
    private readonly string _gameVersion = "1.0.0";
    private string _nickname = "GuardKim";
    private void Start()
    {
        // 설정
        // 1. 버전 : 버전이 다르면 다른 서버로 접속이 된다.
        PhotonNetwork.GameVersion = _gameVersion;
        // 2. 닉네임 : 게임에서 사용할 사용자의 별명(중복 가능 -> 판별을 위해서는 ActorID)
        PhotonNetwork.NickName = _nickname;

        // 방장이 로드한 씬으로 다른 참여자가 똑같이 이동하게끔 동기화 해주는 옵션
        // 방장 : 방을 만든 소유자이자 "마스터 클라이언트" (방마다 한명의 마스터 클라이언트가 존재)
        PhotonNetwork.AutomaticallySyncScene = true;

        // 설정 값들을 이용해 서버 접속 시도
        // 네임 서버 접속 -> 룸 목록이 있는 마스터 서버까지 접속이 된다.
        PhotonNetwork.ConnectUsingSettings();
    }
    // Photon 서버에 접속 후 호출되는 콜백(이벤트) 함수
    public override void OnConnected()
    {
        Debug.Log("네임 서버 접속완료");
        Debug.Log($"{PhotonNetwork.CloudRegion}");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 접속 완료");
        Debug.Log($"InLobby : {PhotonNetwork.InLobby}"); // 로비 입장 유무

        PhotonNetwork.JoinLobby();
        // PhotonNetwork.JoinLobby(TypedLobby.Default); //위의 코드와 같음
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비(채널) 입장 완료!");
        Debug.Log($"InLobby : {PhotonNetwork.InLobby}"); // 로비 입장 유무

        // 랜덤 룸에 들어간다.
        PhotonNetwork.JoinRandomRoom();
    }

    // 룸에 입장한 후 호출되는 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"룸 입장 {PhotonNetwork.InRoom}. : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"플레이어 = {PhotonNetwork.CurrentRoom.PlayerCount}명");

        // 룸에 접속한 사용자 정보
        Dictionary<int, Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
        foreach (KeyValuePair<int, Player> player in roomPlayers)
        {
            Debug.Log($"{player.Value.NickName} : {player.Value.ActorNumber}");
        }
    }

    // 룸 입장에 실패하면 호출되는 함수
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"랜덤 방 입장에 실패했습니다 : {returnCode} : {message}");

        // Room 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true; // 룸 입장 가능 여부
        roomOptions.IsVisible = true; // 로비(채널) 룸 목록에 노출시킬지 여부

        // Room 생성
        PhotonNetwork.CreateRoom("test", roomOptions);
    }

    // 룸 생성에 실패했을 때 호출되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 생성에 실패했습니다. : {returnCode} : {message}");
    }

    // 룸 생성에 성공했을 때 호출되는 함수
    // 룸 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        // 생성된 룸 이름 확인
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }
}

