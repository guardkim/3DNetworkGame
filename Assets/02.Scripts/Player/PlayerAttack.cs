using System;
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

    private void Update()
    {
        // 쉽게 PlayerController에 접근하는 방법
        // _owner.GetAbility<PlayerMove>();
        if (!_photonView.IsMine) return;

        _currentCoolTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && _currentCoolTime <= 0 && TryUseStamina(20f))
        {
            Attack();
            _currentCoolTime = 1f / _owner.Stat.AttackSpeed;
            _isAttacking = false;
            CanRegenStamina();
        }
    }
    public void Attack()
    {
        int rand = Random.Range(0, 2);
        EAttack eAttack = (EAttack)rand;
        _isAttacking = true;
        // _owner.Stat.Stamina -= 20f;
        CanRegenStamina();
        _animator.SetTrigger(eAttack.ToString());
    }
    protected override bool CanRegenStamina()
    {
        return !_isAttacking;
    }
}
