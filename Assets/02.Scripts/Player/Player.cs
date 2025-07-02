using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public enum EPlayerState
{
    Live,
    Death
}

[RequireComponent(typeof(PlayerAbility))]
public class Player : MonoBehaviour, IDamageable
{
    public PlayerStat Stat;
    public int Score;
    private EPlayerState _state = EPlayerState.Live;
    public EPlayerState State => _state;
    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();

    private Animator _animator;
    private CharacterController _characterController;

    public GameObject DamagedEffectPrefab;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    [PunRPC]
    public void Damaged(float damage, int attackerActorNumber)
    {
        if (State == EPlayerState.Death) return;
        Stat.Health = Mathf.Max(0, Stat.Health - damage);
        if (Stat.Health <= 0)
        {
            _state = EPlayerState.Death;

            Instantiate(DamagedEffectPrefab, transform);
            StartCoroutine(Death_Coroutine());

            PhotonView pv = GetComponent<PhotonView>();
            pv.RPC(nameof(OnDie), RpcTarget.All);

            RoomManager.Instance.OnPlayerDeath(pv.Owner.ActorNumber, attackerActorNumber);

            if (pv.IsMine)
            {
                MakeItems(UnityEngine.Random.Range(1,4));
            }
        }
        else
        {
            //RPC로 호출 X
            GetAbility<PlayerShaking>().Shake();
        }
    }

    private void MakeItems(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            // 포톤에서 네트워크 객체의 생명 주기
            // Player : 플레이어가 생성하고, 플레이어가 나가면 자동 삭제(PhotonNetwork.Instantiate/Destroy)
            // Room : 방장이 생성하고, 룸이 생성하고 룸이 없어지면 삭제(PhotonNetwork.InstantiateRoomObject/Destroy)
            int randomNumber = UnityEngine.Random.Range(0, 10);
            if(randomNumber < 3)
            {
                ItemObjectFactory.Instance.RequestCreate(EItemType.Stamina, transform.position);
            }
            else if (randomNumber < 5)
            {
                ItemObjectFactory.Instance.RequestCreate(EItemType.Heal, transform.position);
            }
            else
            {
                ItemObjectFactory.Instance.RequestCreate(EItemType.Score, transform.position);
            }
        }
    }

    private IEnumerator Death_Coroutine()
    {
        var wait = new WaitForSeconds(5f);

        _characterController.enabled = false;

        yield return wait;

        _characterController.enabled = true;

        Stat.Init();
        _state = EPlayerState.Live;
    }

    [PunRPC]
    public void OnDie()
    {
        GetAbility<PlayerHealth>().Die();
    }



    public T GetAbility<T>() where T : PlayerAbility
    {
        Type type = typeof(T);

        if (_abilitiesCache.TryGetValue(typeof(T), out PlayerAbility ability))
        {
            return ability as T;
        }

        // 게으른 초기화/로딩 -> 처음에 곧바로 초기화/로딩을 하는게 아니라 필요할 때만 하는.. 뒤로 미루는 기법
        if (TryGetComponent(out T newAbility))
        {
            _abilitiesCache[newAbility.GetType()] = newAbility;
            return newAbility;
        }


        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다");
    }

}
