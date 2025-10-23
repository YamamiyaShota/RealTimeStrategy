using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCore : MonoBehaviour, IDamage
{
    public CharacterType Type => _type;
    [Header("ステータス")]
    [SerializeField] private int _hp;
    [SerializeField] private CharacterType _type;

    public bool AddDamage(int atk)
    {
        _hp -= atk;
        if(_hp <= 0)
        {
            _hp = 0;
            // TODO ゲームオーバーの処理を呼び出す
            return true;
        }
        return false;
    }
}