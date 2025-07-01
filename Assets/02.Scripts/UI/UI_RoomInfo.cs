using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomInfo : MonoBehaviour
{
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI PlayerCountText;
    public Button ExitButton;

    private void Start()
    {
        RoomManager.Instance.OnRoomDataChanged += Refresh;
        Refresh();
    }
    public void Refresh()
    {
        Room room = RoomManager.Instance.Room;
        if (room == null) return;
        RoomNameText.text = $"방 이름 : {room.Name}";
        PlayerCountText.text = $"{room.PlayerCount.ToString()} / {room.MaxPlayers}";
    }
    public void OnClickExitButton()
    {
        Exit();
    }
    private void Exit()
    {
        Application.Quit();

        // 에디터에서 테스트할 경우엔 아래 코드도 추가하면 좋습니다.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
