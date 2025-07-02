
using UnityEngine;

public class BearDeathState : IState<BearController>
{
    private BearController _bear;

    public void OnEnter(BearController bear)
    {
        _bear = bear;
        Debug.Log("사망 상태 진입");

        // 사망 애니메이션 실행
        _bear.Animator.SetTrigger("Die");

        // 물리적 충돌 및 이동 중지
        _bear.Agent.enabled = false;
        // 다른 컴포넌트 비활성화 (예: Collider)
        // var collider = _bear.GetComponent<Collider>();
        // if (collider != null) collider.enabled = false;

        // 리스폰 코루틴 시작 요청
        _bear.Respawn();

        // 일정 시간 후 오브젝트 비활성화 (선택 사항)
        // MonoBehaviour.Destroy(_bear.gameObject, 5f); // 5초 후 파괴
        _bear.gameObject.SetActive(false); // 즉시 비활성화 (리스폰 시 다시 활성화)
    }

    public void OnUpdate()
    {
        // 사망 상태에서는 아무것도 하지 않음
    }

    public void OnExit()
    { 
        // 리스폰될 때 호출됨
        Debug.Log("사망 상태 종료 (리스폰)");
        _bear.Agent.enabled = true;
        // 비활성화했던 컴포넌트 다시 활성화
        // var collider = _bear.GetComponent<Collider>();
        // if (collider != null) collider.enabled = true;
    }
}
