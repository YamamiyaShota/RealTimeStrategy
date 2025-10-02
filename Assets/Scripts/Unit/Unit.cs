using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private GameObject _character;
    private NavMeshAgent _agent;

    [SerializeField] private float _arraivedDistance = 1.5f;
    [SerializeField] private float _followDistance = 1;

    private Vector3 _targetPosition;

    private void Start()
    {
        _targetPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
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
}