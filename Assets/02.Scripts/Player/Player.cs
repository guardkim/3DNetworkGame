using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum EPlayerState
{
    Live,
    Death
}
public class Player : MonoBehaviour, IDamageable
{
    public PlayerStat Stat;
    private EPlayerState _state = EPlayerState.Live;
    public EPlayerState State => _state;
    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();

    private Animator _animator;
    private CharacterController _characterController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    [PunRPC]
    public void Damaged(float damage)
    {
        if (State == EPlayerState.Death) return;
        Stat.Health = Mathf.Max(0, Stat.Health - damage);
        if (Stat.Health <= 0)
        {
            _state = EPlayerState.Death;


            StartCoroutine(Death_Coroutine());

            PhotonView pv = GetComponent<PhotonView>();
            pv.RPC(nameof(OnDie), RpcTarget.All);
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
