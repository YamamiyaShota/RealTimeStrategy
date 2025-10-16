using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IUnit, IState
{
    public int HP => _hp;
    public int ATK => _atk;
    public int DF => _df;
    public float AttackInterval => _attackInterval;
    public float AttackRange => _attackRange;
    public float MoveSpeed => _moveSpeed;

    public StateType GetCurrentState => _currentState;
    public StateType GetNextState => _nextState;
    public bool IsStateEnd { get; private set; } = false;

    [Header("ステータス")]
    [SerializeField] private int _hp;
    [SerializeField] private int _atk;
    [SerializeField] private int _df;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackInterval;
    [SerializeField] private float _attackRange;

    [Header("AI設定")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _arraiveDistance = 1.5f;
    [SerializeField] private float _followDistance = 1;

    [SerializeField]private StateType _currentState;
    private StateType _nextState;
    private IState _state => this;

    private MoveType _moveType;
    private GameObject _attackTarget;
    private Transform _followUnit;
    private Vector3 _targetPosition;
    private float _attackTimer;
    private bool _isAttackTargetDead;


    private void Start()
    {
        _targetPosition = transform.position;
        if (_agent == null)
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        _attackTimer = _attackInterval;
    }

    private void Update()
    {
        _state.Update();
    }

    void IState.Update()
    {
        switch (_currentState)
        {
            case StateType.Idle:
                break;
            case StateType.Move:
                Move();
                break;
            case StateType.Attack:
                Attack();
                break;
            case StateType.Death:
                break;
        }

        if (IsStateEnd)
        {
            _currentState = _nextState;
            return;
        }
    }

    public bool AddDamage(int atk)
    {
        _hp -= atk - _df;
        if(_hp <= 0)
        {
            Destroy(gameObject);
            return true;
        }

        if(_currentState == StateType.Idle)
        {
            SetAttackTarget(FindNearEnemy());
            SetState(StateType.Attack);
        }

        return false;
    }

    public void SetFollowUnit(Transform followUnit)
    {
        _followUnit = followUnit;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void SetState(StateType currentState, StateType nextState = StateType.Idle)
    {
        _currentState = currentState;
        _nextState = nextState;
        Debug.Log($"今の状態:{_currentState} \n次の状態:{_nextState}");
        OnStateBegine();
    }

    public void OnStateBegine()
    {
        IsStateEnd = false;
        Debug.Log($"{name}の{GetCurrentState}を開始します");
    }

    public void OnStateEnd()
    {
        IsStateEnd = true;
        Debug.Log($"{name}の{GetCurrentState}を終了します");
    }

    public void SetAttackTarget(GameObject attackTarget)
    {
        _attackTarget = attackTarget;
    }

    public void SetMoveType(MoveType moveType)
    {
        _moveType = moveType;
    }

    private void Move()
    {
        //if (IsStateEnd)
        //{
        //    OnStateBegine();
        //}

        switch (_moveType)
        {
            case MoveType.Parent:
                _agent.SetDestination(_targetPosition);
                if(_agent.remainingDistance == 0)
                {
                    _agent.isStopped = true;
                }
                else
                {
                    _agent.isStopped = false;
                }
                //Debug.Log($"{_agent.remainingDistance} < {_arraiveDistance}");
                break;
            case MoveType.Child:
                _agent.SetDestination(_followUnit.position);
                if(_followUnit.TryGetComponent(out NavMeshAgent component))
                {
                    _agent.isStopped = component.isStopped;
                }
                else if (_agent.remainingDistance > _followDistance)
                {
                    _agent.isStopped = false;
                    this.transform.LookAt(_followUnit.position);
                }
                break;
        }

        //if (_agent.remainingDistance < _arraiveDistance && _followUnit != null)
        //{
        //    _agent.isStopped = true;
        //}
        //else if (_agent.remainingDistance > _followDistance)
        //{
        //    _agent.isStopped = false;
        //}

        if (_agent.isStopped)
        {
            _followUnit = null;
            OnStateEnd();
        }
    }

    private void Attack()
    {
        //if (IsStateEnd)
        //{
        //    OnStateBegine();
        //}
        var distance = Vector3.Distance(_attackTarget.transform.position, this.transform.position);
        if(distance > _attackRange)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_attackTarget.transform.position);
            //Debug.Log($"{distance} > {_attackRange}");
            //Debug.Log(_agent.isStopped);
            return;
        }
        else
        {
            _agent.isStopped = true;
        }

        _attackTimer -= Time.deltaTime;

        if(_attackTimer <= 0)
        {
            if (_attackTarget.TryGetComponent<IUnit>(out IUnit enemy))
            {
                _isAttackTargetDead = enemy.AddDamage(_atk);
            }
            _attackTimer = _attackInterval;
        }

        if(_isAttackTargetDead)
        {
            OnStateEnd();
        }
    }

    private GameObject FindNearEnemy()
    {
        GameObject[] enemy =  GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearEnemy = null;
        float distance = Mathf.Infinity;
        Vector3 position = this.transform.position;
        foreach(GameObject enemyObj in enemy)
        {
            float targetDistance = Vector3.Distance(enemyObj.transform.position, position);
            if(targetDistance < distance)
            {
                nearEnemy = enemyObj;
                distance = targetDistance;
            }
        }
        return nearEnemy;
    }
}

public enum MoveType
{
    Parent,
    Child,
}