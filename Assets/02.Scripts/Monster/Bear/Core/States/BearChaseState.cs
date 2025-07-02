
using UnityEngine;

public class BearChaseState : IState<BearController>
{
    private BearController _bear;

    public void OnEnter(BearController bear)
    {
        _bear = bear;
        _bear.Animator.SetBool("IsWalking", true);
        Debug.Log("Chase 상태 진입");
    }

    public void OnUpdate()
    {
        if (_bear.Player == null)
        {
            _bear.StateMachine.SetState(typeof(BearPatrolState));
            return;
        }

        float distanceToPlayer = Vector3.Distance(_bear.transform.position, _bear.Player.position);

        // 공격 범위에 들어오면 Attack 상태로 변경
        if (distanceToPlayer <= _bear.attackRange)
        {
            _bear.StateMachine.SetState(typeof(BearAttackState));
            return;
        }

        // 감지 범위를 벗어나면 Patrol 상태로 변경 (플레이어를 놓침)
        if (distanceToPlayer > _bear.detectionRange)
        {
            _bear.StateMachine.SetState(typeof(BearPatrolState));
            return;
        }

        // 플레이어를 계속 추적
        _bear.Agent.SetDestination(_bear.Player.position);
    }

    public void OnExit()
    {
        Debug.Log("Chase 상태 종료");
    }
}
