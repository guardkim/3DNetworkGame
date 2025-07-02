using UnityEngine;

public class BearAttackState : IState<BearController>
{
    private BearController _bear;

    public void OnEnter(BearController bear)
    {
        _bear = bear;
        _bear.Agent.ResetPath(); // 공격 시에는 제자리에 멈춤
        _bear.Animator.SetTrigger("Attack");
        Debug.Log("Attack 상태 진입");
        // 플레이어를 바라보게 함
        if (_bear.Player != null) {
            _bear.transform.LookAt(_bear.Player.position);
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

        // TODO: 공격 애니메이션 이벤트와 연동하여 실제 데미지 처리
        // 예: 애니메이션의 특정 프레임에서 Hit() 함수 호출
    }

    public void OnExit()
    {
        Debug.Log("Attack 상태 종료");
    }
}