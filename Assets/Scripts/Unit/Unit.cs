using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IUnit
{
    [Header("ステータス")]
    [SerializeField] private int _hp;
    [SerializeField] private int _atk;
    [SerializeField] private int _df;
    [SerializeField] private float _attackInterval;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackRange;

    [Header("AI設定")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _arraivedDistance = 1.5f;
    [SerializeField] private float _followDistance = 1;


    public int HP => _hp;
    public int ATK => _atk;
    public int DF => _df;
    public float AttackInterval => _attackInterval;
    public float MoveSpeed => _moveSpeed;
    public float AttackRange => _attackRange;

    private GameObject _character;
    private Vector3 _targetPosition;

    private void Start()
    {
        _targetPosition = transform.position;
        if(_agent == null)
        {
            _agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Update()
    {
        if(_character != null)
        {
            _agent.SetDestination(_character.transform.position);
        }
        else if(_targetPosition != null)
        {
            _agent.SetDestination(_targetPosition);
        }

        if(_agent.remainingDistance < _arraivedDistance)
        {
            _agent.isStopped = true;
            _character = null;
        }
        else if(_agent.remainingDistance > _followDistance)
        {
            _agent.isStopped = false;
        }
    }

    public void SetCharacter(GameObject character)
    {
        _character = character;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void AddDamage(int atk)
    {
        _hp -= atk - _df;
    }
}