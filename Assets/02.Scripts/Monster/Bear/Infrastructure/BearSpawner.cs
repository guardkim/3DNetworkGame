
using UnityEngine;

public class BearSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject bearPrefab;
    [SerializeField] private int maxSpawnCount = 5;
    [SerializeField] private float spawnRadius = 20f; // 스폰 반경

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform[] spawnPoints;


    private void Start()
    {
        for (int i = 0; i < maxSpawnCount; i++)
        {
            SpawnBear();
        }
    }

    private void SpawnBear()
    {
        // 랜덤 위치 계산
        int randomNumber = UnityEngine.Random.Range(0, spawnPoints.Length);
        Vector3 randomPos = spawnPoints[randomNumber].position + Random.insideUnitSphere * spawnRadius;
        randomPos.y = transform.position.y; // 높이는 스포너와 동일하게

        // NavMesh 상에서 가장 가까운 위치를 찾음
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPos, out UnityEngine.AI.NavMeshHit hit, spawnRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            GameObject bearObject = Instantiate(bearPrefab, hit.position, Quaternion.identity, transform);
            BearController bearController = bearObject.GetComponent<BearController>();

            if (bearController != null)
            {
                bearController.Initialize(this, patrolPoints, hit.position);
            }
            else
            {
                Debug.LogError("Bear Prefab에 BearController가 없습니다.");
                Destroy(bearObject);
            }
        }
        else
        {
            Debug.LogWarning("스폰 가능한 NavMesh 위치를 찾지 못했습니다.");
        }
    }

    // Bear가 죽었을 때 호출하는 리스폰 함수
    public void Respawn(BearController bear)
    {
        Debug.Log($"{bear.name}의 리스폰을 처리합니다.");
        Destroy(bear.gameObject); // 기존 오브젝트 파괴
        SpawnBear(); // 새로운 곰 스폰
    }
}
