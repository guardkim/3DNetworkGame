
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class BearController : MonoBehaviour, IDamageable
{
    public StateMachine<BearController> StateMachine { get; private set; }

    // 컴포넌트
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }

    [Header("Ranges")]
    public float detectionRange = 15f; // 플레이어 감지 범위
    public float attackRange = 2f;    // 공격 범위

    // 타겟
    public Transform Player { get; private set; }

    // 순찰 및 스폰
    public Vector3 SpawnPosition { get; private set; }
    public Transform[] PatrolPoints { get; private set; }

    // 체력
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;
    public bool IsDead => _currentHealth <= 0;

    // 리스폰
    [Header("Respawn")]
    [SerializeField] private float respawnTime = 30f;

    private BearSpawner _spawner; // 자신을 생성한 스포너

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();

        // FSM 초기화 (상태 추가)
        StateMachine = new StateMachine<BearController>(this);
        StateMachine.AddState(typeof(BearIdleState), new BearIdleState());
        StateMachine.AddState(typeof(BearPatrolState), new BearPatrolState());
        StateMachine.AddState(typeof(BearChaseState), new BearChaseState());
        StateMachine.AddState(typeof(BearAttackState), new BearAttackState());
        StateMachine.AddState(typeof(BearDeathState), new BearDeathState());
    }

    public void Initialize(BearSpawner spawner, Transform[] patrolPoints, Vector3 spawnPosition)
    {
        _spawner = spawner;
        PatrolPoints = patrolPoints;
        SpawnPosition = spawnPosition;
        _currentHealth = maxHealth;

        // 임시로 플레이어 찾기 (실제 게임에서는 더 좋은 방법 사용 권장)
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // FSM 시작 상태 설정
        StateMachine.SetState(typeof(BearIdleState));
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        _currentHealth -= damage;
        Debug.Log($"곰이 {damage}의 데미지를 입었습니다. 현재 체력: {_currentHealth}");

        if (IsDead)
        {
            StateMachine.SetState(typeof(BearDeathState));
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        // 스포너에게 리스폰 요청
        _spawner.Respawn(this);
    }
    public void Damaged(float damage, int attackerActorNumber)
    {
        StateMachine.SetState(typeof(BearAttackState));
        _currentHealth -= damage;
    }
}
