using System;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class ScoreItemSpawner : MonoBehaviour
{
    public float Interval; // 몇초마다 생성할 것이냐
    private float _intervalTimer = 0;
    public float Range; // 랜덤한 범위

    private void Start()
    {
        Interval = UnityEngine.Random.Range(10, 20);
        Range =    UnityEngine.Random.Range(1, 10);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _intervalTimer += Time.deltaTime;

        if (_intervalTimer >= Interval)
        {
            _intervalTimer = 0;
            Vector3 randomPosition = transform.position + UnityEngine.Random.insideUnitSphere * Range;
            randomPosition.y = 3f;

            ItemObjectFactory.Instance.RequestCreate(EItemType.Score, randomPosition);
        }
    }
}
