using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetDetector : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IUnit unit = other.gameObject.GetComponentInParent<IUnit>();
            _enemy.SetAttackTarget(other.gameObject.transform.position, unit);
        }
    }
}
