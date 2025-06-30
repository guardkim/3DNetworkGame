using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : Singleton<SpawnPoints>
{
    protected override void Awake()
    {
        base.Awake();
    }

    [SerializeField]
    private List<Transform> _spawnPoints;

    public Vector3 GetRandomSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
    }
}
