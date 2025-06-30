using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttack _attack;
    private void Start()
    {
        _attack = GetComponentInParent<PlayerAttack>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // 자기 자신과 부딛혔다면 아무고또 안한다.
        if (other.transform == _attack.transform)
        {
            return;
        }

        if(other.GetComponent<IDamageable>() != null)
        {
            _attack.Hit(other);
        }
    }
}
