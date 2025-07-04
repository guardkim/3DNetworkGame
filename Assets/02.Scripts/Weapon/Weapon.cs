using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttack _attack;
    private void Start()
    {
        _attack = GetComponentInParent<PlayerAttack>();

        ScoreManager.Instance.OnDataChanged += Refresh;

        Refresh();
    }
    private void Refresh()
    {
        int score = ScoreManager.Instance.Score;
        int factor = 1 + score / 10000;

        transform.localScale = new Vector3(factor, factor, factor);
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
