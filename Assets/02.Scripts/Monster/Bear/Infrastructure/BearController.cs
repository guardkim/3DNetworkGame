
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Photon.Pun;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(PhotonView))]
public class BearController : MonoBehaviourPunCallbacks, IDamageable, IPunObservable
{
    public StateMachine<BearController> StateMachine { get; private set; }

    // 컴포넌트
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    private PhotonView _photonView;

    [Header("Ranges")]
    public float detectionRange = 15f; // 플레이어 감지 범위
    public float attackRange = 2f;    // 공격 범위

    // 타겟
    public Transform Player { get; set; }

    // 순찰 및 스폰
    public Vector3 SpawnPosition { get; private set; }
    public Transform[] PatrolPoints { get; private set; }

    // 체력
    [Header("Stats")]
    [SerializeField] public float maxHealth = 100f;
    private float _currentHealth;
    public bool IsDead => _currentHealth <= 0;

    // 리스폰
    [Header("Respawn")]
    [SerializeField] private float respawnTime = 30f;

    private BearSpawner _spawner; // 자신을 생성한 스포너

    public Slider HealthSlider;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();

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

        // FSM 시작 상태 설정
        StateMachine.SetState(typeof(BearIdleState));
    }
    public void Refresh()
    {
        Debug.Log($"{_currentHealth} 지금 피");
        HealthSlider.value = _currentHealth / maxHealth;
        Debug.Log($"{maxHealth} 최대 피");
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 마스터 클라이언트가 체력 정보를 보냄
            stream.SendNext(_currentHealth / maxHealth);
            Debug.Log($"[PhotonSerializeView] MasterClient Sending Health: {_currentHealth}");
        }
        else
        {
            // 다른 클라이언트가 체력 정보를 받음
            _currentHealth = (float)stream.ReceiveNext();
            Debug.Log($"[PhotonSerializeView] Client Receiving Health: {_currentHealth}");
            Refresh(); // 받은 체력으로 UI 업데이트
        }
    }

    [PunRPC]
    public void RPC_SetInitialHealth(float health)
    {
        _currentHealth = health;
        Refresh();
        Debug.Log($"[RPC] Initial Health Set to: {_currentHealth}");
    }

    public void RequestRespawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _spawner.Respawn(this);
        }
        else
        {
            _photonView.RPC("RPC_RequestRespawn", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RPC_RequestRespawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _spawner.Respawn(this);
        }
    }

    [PunRPC]
    public void Damaged(float damage, int attackerActorNumber)
    {
        _photonView.RPC(nameof(RPC_SetAnimatorTrigger), RpcTarget.All, "Attacked");

        if (PhotonNetwork.IsMasterClient)
        {
            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth); // 체력을 0과 maxHealth 사이로 클램프
            if (IsDead)
            {
                StateMachine.SetState(typeof(BearDeathState));
            }
        }
        Refresh();
        Debug.Log($"곰이 {damage}의 데미지를 입었습니다. 현재 체력: {_currentHealth}");
    }

    [PunRPC]
    private void RPC_SetAnimatorTrigger(string triggerName)
    {
        Animator.SetTrigger(triggerName);
    }
}
