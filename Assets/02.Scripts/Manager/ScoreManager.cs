using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;

public class ScoreManager : SingletonPhoton<ScoreManager>
{
    private int _score = 0;
    public int Score => _score;
    private int _killCount;
    public int KillCount => _killCount;

    private Dictionary<string, int> _scores = new Dictionary<string, int>();
    public Dictionary<string, int> Scores => _scores;

    public event Action OnDataChanged;


    public void Refresh()
    {
        int finalScore = _score + _killCount * 5000;

        // 점수에 따라 무기 크기 조정
        float scale = 1.0f + (finalScore / 10000) * 0.1f;
        // SetWeaponScale(scale);

        // 커스텀 프로퍼티에 최종 점수 반영
        Hashtable hashTable = new Hashtable();
        hashTable["Score"] = finalScore;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
    }
    public override void OnJoinedRoom()
    {
        // 방에 들어가면 '내 점수가 0점이다'라는 내용으로
        // 커스텀 프로퍼티를 초기화 해준다.
        Refresh();
    }
    public void AddKill()
    {
        _killCount++;
        Refresh();
    }
    public void AddScore(int score)
    {
        _score += score;
        Refresh();
    }

    public void AddPlayerScore(string playerName, int score)
    {
        if (!_scores.ContainsKey(playerName))
            _scores[playerName] = 0;

        _scores[playerName] += score;
        OnDataChanged?.Invoke();
    }
    protected override void Awake()
    {
        base.Awake();
    }

    // 특정 ActorNumber에게만 점수를 주는 RPC 호출
    [PunRPC]
    public void StealScore(int fromActorNumber)
    {
        var target = GetPlayerByActorNumber(fromActorNumber);
        if (target == null || !target.CustomProperties.ContainsKey("Score"))
            return;

        int victimScore = (int)target.CustomProperties["Score"];
        int stealAmount = victimScore / 2;
        int dropAmount = victimScore - stealAmount;

        AddScore(stealAmount);

        // 점수 오브젝트 뿌리기
        SpawnScoreDrops(dropAmount, target);
    }

    private void SpawnScoreDrops(int amount, Photon.Realtime.Player victim)
    {
        // 점수 오브젝트를 해당 플레이어 위치에 생성하는 로직
        // 예시이므로 필요시 Instantiate 처리
        Debug.Log($"Spawn {amount} points worth of score objects at {victim.NickName}'s position");
    }

    public void KillPlayer(int victimActorNumber)
    {
        photonView.RPC(nameof(StealScore), RpcTarget.All, victimActorNumber);
        AddKill();
    }

    private Photon.Realtime.Player GetPlayerByActorNumber(int actorNumber)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == actorNumber)
                return player;
        }
        return null;
    }

    // 플레이어의 커스텀 프로퍼티가 변경되면 호출되는 콜백 함수
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable hashtable)
    {
        // Debug.Log($"Player {targetPlayer.NickName}_{targetPlayer.ActorNumber}의 점수 : {hashtable["Score"]}");

        var roomPlayers = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player player in roomPlayers)
        {
            if(player.CustomProperties.ContainsKey("Score"))
                _scores[$"{player.NickName}_{player.ActorNumber}"] = (int)player.CustomProperties["Score"];
        }
        OnDataChanged?.Invoke();
    }
}
