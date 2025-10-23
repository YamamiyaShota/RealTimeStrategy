using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseUnit
{
    [SerializeField] private SphereCollider _sphereCollider;

    private IDamage _unit;
    private Vector3 _attackTargetPosition;
    private float _attackTimer;
    private bool _isAttackTargetDead;

    private void Start()
    {
        if(_sphereCollider != null)
        {
            _sphereCollider.radius = _attackRange;
        }
    }

    public override void Update()
    {
        if (_attackTargetPosition != Vector3.zero)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (_isAttackTargetDead)
        {
            return;
        }

        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0)
        {
            _isAttackTargetDead = _unit.AddDamage(_atk);
            _attackTimer = _attackInterval;
        }
    }

    public override bool AddDamage(int atk)
    {
        _hp -= atk - _df;
        if (_hp <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public void SetAttackTarget(Vector3 targetPosition, IDamage unit)
    {
        _attackTargetPosition = targetPosition;
        _unit = unit;
    }
}
