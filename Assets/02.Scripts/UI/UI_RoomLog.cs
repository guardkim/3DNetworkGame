using System;
using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    public TextMeshProUGUI LogTextUI;

    private string _logMessage = "방에 입장했습니다.";

    private void Start()
    {
        RoomManager.Instance.OnPlayerEnter += PlayerEnterLog;
        RoomManager.Instance.OnPlayerExit += PlayerExitLog;
        RoomManager.Instance.OnPlayerDied += PlayerDeathLog;
        Refresh();
    }
    private void Refresh()
    {
        LogTextUI.text = _logMessage;
    }

    public void PlayerEnterLog(string playerName)
    {
        // 관리는 Manager가... UI가 서버 로직을 알면 스마트 UI
        _logMessage += $"\n<color=green>{playerName}</color>님이 <color=blue>입장</color>하였습니다.";
        Refresh();
    }
    public void PlayerExitLog(string playerName)
    {
        _logMessage += $"\n<color=green>{playerName}</color>님이 <color=red>퇴장</color>하였습니다.";
        Refresh();
    }
    public void PlayerDeathLog(string playerName, string attackerName)
    {
        if (string.Equals(attackerName, "지형지물"))
        {
            _logMessage += $"\n<color=green>{playerName}</color>님이 <color=red>낙사</color>하였습니다.";
        }
        else
        {
            _logMessage += $"\n<color=red>{attackerName}</color>님이 <color=green>{playerName}</color>님을 <color=red>처치</color>하였습니다.";
        }
        Refresh();
    }
}
