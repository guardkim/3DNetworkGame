using UnityEngine;
public class AttackBehavour : StateMachineBehaviour
{
    private PlayerAttack _attack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_attack == null)
            _attack = animator.GetComponentInParent<PlayerAttack>(); // 또는 animator.GetComponent<PlayerAttack>();

        _attack.ActiveCollider(); // 공격 시작
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_attack == null)
            _attack = animator.GetComponentInParent<PlayerAttack>();

        _attack.DeActiveCollider(); // 공격 종료
        //_attack.OnAttackEnd(); // 상태 리셋
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
