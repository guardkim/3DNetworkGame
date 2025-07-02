using UnityEngine;

public class BearIdleState : IState<BearController>
{
    private BearController _bear;
    private float _idleTimer;

    public void OnEnter(BearController bear)
    {
        _bear = bear;
        _idleTimer = 0f;
        _bear.Animator.SetBool("IsWalking", false);
        _bear.Agent.ResetPath();
        Debug.Log("Idle 상태 진입");
    }

    public void OnUpdate()
    {
        // 플레이어 감지
        if (IsPlayerInDetectionRange())
        {
            _bear.StateMachine.SetState(typeof(BearChaseState));
            return;
        }

        // 5초 후 순찰 상태로 전환
        _idleTimer += Time.deltaTime;
        if (_idleTimer >= 5f)
        {
            _bear.StateMachine.SetState(typeof(BearPatrolState));
        }
    }

    public void OnExit()
    {
        Debug.Log("Idle 상태 종료");
    }

    private bool IsPlayerInDetectionRange()
    {
        if (_bear.Player == null) return false;
        float distanceToPlayer = Vector3.Distance(_bear.transform.position, _bear.Player.position);
        return distanceToPlayer <= _bear.detectionRange;
    }
}