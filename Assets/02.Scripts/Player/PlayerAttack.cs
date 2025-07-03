using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public enum EAttack
{
    Attack1 = 0,
    Attack2 = 1,
    Attack3 = 2
}
public class PlayerAttack : PlayerAbility
{
    [Header("공격")]
    private float _currentCoolTime;
    private bool _isAttacking = false;

    public Collider WeaponCollider;
    public GameObject AttackEffectPrefab;

    private void Start()
    {
        DeActiveCollider();
    }

    // 위치/회전처럼 상시로 확인이 필요한 데이터 동기화 : IPunObservable(OnPhotonSerializeView)
    // 트리거/공격/피격 처럼 간헐적으로 특정한 이벤트가 발생했을때의 변화된 데이터 동기화 : RPC
    // RPC : Remote Procedure Call
    //      ㄴ 물리적으로 떨어져있는 다른 디바이스의 함수를 호출하는 기능
    //      ㄴ RPC 함수를 호출하면 네트워크를 통해 다른 사용자의 스크립트에서 해당 함수가 호출된다.
    private void Update()
    {
        // 쉽게 PlayerController에 접근하는 방법
        // _owner.GetAbility<PlayerMove>();
        if (_photonView.IsMine == false || _owner.State == EPlayerState.Death) return;

        _currentCoolTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && _currentCoolTime <= 0 && TryUseStamina(20f))
        {
            Attack();
            _currentCoolTime = 1f / _owner.Stat.AttackSpeed;
            _isAttacking = false;
        }
    }
    public void Attack()
    {
        _isAttacking = true;

        //1. 일반 메서드 호출 방식
        //PlayAttackAnimation(Random.Range(1, 4));

        //2. RPC 메서드 호출 방식
        _photonView.RPC(nameof(PlayAttackAnimation), RpcTarget.All, Random.Range(1, 4));

    }

    public void ActiveCollider()
    {
        WeaponCollider.enabled = true;
    }
    public void DeActiveCollider()
    {
        WeaponCollider.enabled = false;
    }
    [PunRPC]
    private void PlayAttackAnimation(int randomNumber)
    {
        _animator.SetTrigger($"Attack{randomNumber}");
    }
    protected override bool CanRegenStamina()
    {
        return !_isAttacking;
    }

    public void Hit(Collider other)
    {
        if (_photonView.IsMine == false || _owner.State == EPlayerState.Death)
        {
            return;
        }
        if (other.GetComponentInParent<IDamageable>() == null) return;
        DeActiveCollider();

        Instantiate(AttackEffectPrefab, other.transform);


        // RPC로 호출해야지 다른 사람의 게임오브젝트들도 이 함수가 실행된다.
        // damageableObject.Damaged(_owner.Stat.Damage);
        PhotonView otherPhotonView = other.GetComponent<PhotonView>();
        otherPhotonView.RPC(nameof(IDamageable.Damaged), RpcTarget.All, _owner.Stat.Damage, _photonView.Owner.ActorNumber);

    }
}
