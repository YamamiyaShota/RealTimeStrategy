using System.Collections;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour, IDamage, IState
{
    public virtual StateType GetCurrentState => _currentState;
    public virtual StateType GetNextState => _nextState;
    public virtual bool IsStateEnd { get; protected set; } = false;

    [Header("ステータス")]
    [SerializeField] protected int _hp;
    [SerializeField] protected int _atk;
    [SerializeField] protected int _df;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackInterval;
    [SerializeField] protected float _attackRange;

    [SerializeField] protected StateType _currentState;
    protected StateType _nextState;

    public virtual bool AddDamage(int atk)
    {
        _hp -= (atk - _df > 0) ? atk - _df : 1;
        if (_hp <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public void SetState(StateType currentState, StateType nextState = StateType.Idle)
    {
        _currentState = currentState;
        _nextState = nextState;
    }

    public  void OnStateBegine()
    {
        IsStateEnd = false;
    }

    public void OnStateEnd()
    {
        IsStateEnd = true;
    }

    public abstract void Update();
}
