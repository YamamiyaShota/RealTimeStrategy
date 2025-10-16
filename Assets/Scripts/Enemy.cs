using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IUnit
{
    public int HP => _hp;

    public int ATK => _atk;

    public int DF => _df;
    public float MoveSpeed => _moveSpeed;

    public float AttackInterval => _attackInterval;

    public float AttackRange => _attackRange;

    [SerializeField] private int _hp;
    [SerializeField] private int _atk;
    [SerializeField] private int _df;
    [SerializeField] private float _attackInterval;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackRange;

    public bool AddDamage(int atk)
    {
        _hp -= atk - _df;
        Debug.Log($"{name}は{atk - _df}のダメージを受けた");
        Debug.Log($"現在の{name}のHPは{_hp}");
        if(_hp <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
