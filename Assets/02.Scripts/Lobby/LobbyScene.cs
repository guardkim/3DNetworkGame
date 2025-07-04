using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    public TMP_InputField NicknameInputField;
    public TMP_InputField RoomNameInputField;

    public GameObject MaleCharacter;
    public GameObject FemaleCharacter;

    public void OnClickMakeRoomButton()
    {
        MakeRoom();
    }

    public void OnClickMaleCharacter()
    {
        MaleCharacter.SetActive(true);
        FemaleCharacter.SetActive(false);
    }
    public void OnClickFemaleCharacter()
    {
        FemaleCharacter.SetActive(true);
        MaleCharacter.SetActive(false);
    }
    private void MakeRoom()
    {
        if (MaleCharacter.activeInHierarchy) PhotonServerManager.Instance.IsMale = true;
        else PhotonServerManager.Instance.IsMale = false;

        string nickname = NicknameInputField.text;
        string roomName = RoomNameInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            return;
        }
        PhotonNetwork.NickName = nickname;


        // Room 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true; // 룸 입장 가능 여부
        roomOptions.IsVisible = true; // 로비(채널) 룸 목록에 노출시킬지 여부

        // Room 생성
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
}
