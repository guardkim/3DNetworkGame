using Photon.Pun;
using UnityEngine;

public class BearAttackState : IState<BearController>
{
    private BearController _bear;
    private float _lastAttackTime;
    private float _attackCooldown = 2f; // 2초마다 공격

    public void OnEnter(BearController bear)
    {
        _bear = bear;
        if (_bear.Agent.isActiveAndEnabled == true)
        {
            _bear.Agent.ResetPath(); // 공격 시에는 제자리에 멈춤
            Attack();
        }

    }

    public void OnUpdate()
    {
        if (_bear.Player == null) {
            _bear.StateMachine.SetState(typeof(BearPatrolState));
            return;
        }

        float distanceToPlayer = Vector3.Distance(_bear.transform.position, _bear.Player.position);

        // 플레이어가 공격 범위를 벗어나면 Chase 상태로 변경
        if (distanceToPlayer > _bear.attackRange)
        {
            _bear.StateMachine.SetState(typeof(BearChaseState));
            return;
        }

        // 쿨다운이 지났으면 다시 공격
        if (Time.time - _lastAttackTime > _attackCooldown)
        {
            Attack();
        }
    }

    public void OnExit()
    {
        Debug.Log("Attack 상태 종료");
    }

    private void Attack()
    {
        _lastAttackTime = Time.time;
        _bear.GetComponent<PhotonView>().RPC("RPC_SetAnimatorTrigger", RpcTarget.All, "Attack");
        Debug.Log("곰이 플레이어를 공격합니다.");

        // 플레이어를 바라보게 함
        if (_bear.Player != null) {
            _bear.transform.LookAt(_bear.Player.position);
        }
    }
}
