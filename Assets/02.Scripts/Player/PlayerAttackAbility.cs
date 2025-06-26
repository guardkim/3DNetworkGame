using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttackAbility : MonoBehaviour
{
    private Animator _animator;
    private float _attackTimer = 0f;
    private const float ATTACK_DURATION = 5.6f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
            return;
        }

        Attack();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("Attack1");
            _attackTimer = ATTACK_DURATION;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _animator.SetTrigger("Attack2");
            _attackTimer = ATTACK_DURATION;
        }
        else if (Input.GetMouseButtonDown(2))
        {
            _animator.SetTrigger("Attack3");
            _attackTimer = ATTACK_DURATION;
        }
    }
}
