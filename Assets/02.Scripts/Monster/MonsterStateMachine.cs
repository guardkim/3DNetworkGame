using UnityEngine;

public class MonsterStateMachine
{
    private IMonsterState _currentState;

    public void SetState(IMonsterState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Update()
    {
        _currentState?.Update();
    }
}
