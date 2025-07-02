
using System.Collections.Generic;

public class StateMachine<T> where T : class
{
    private T _owner;
    private IState<T> _currentState;
    private Dictionary<System.Type, IState<T>> _states = new Dictionary<System.Type, IState<T>>();

    public IState<T> CurrentState => _currentState;

    public StateMachine(T owner)
    {
        _owner = owner;
        _currentState = null;
    }

    public void AddState(System.Type type, IState<T> state)
    {
        _states[type] = state;
    }

    public void SetState(System.Type type)
    {
        if (_states.ContainsKey(type) == false) 
        {
            UnityEngine.Debug.LogError($"[StateMachine] {_owner.GetType().Name}: 상태 '{type.Name}'를 찾을 수 없습니다.");
            return;
        }

        _currentState?.OnExit();
        _currentState = _states[type];
        _currentState.OnEnter(_owner);
    }

    public void Update()
    {
        _currentState?.OnUpdate();
    }
}
