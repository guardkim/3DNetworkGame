
public interface IState<T> where T : class
{
    void OnEnter(T owner);
    void OnUpdate();
    void OnExit();
}
