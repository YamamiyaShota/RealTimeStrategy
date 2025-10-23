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
            IDamage unit = other.gameObject.GetComponentInParent<IDamage>();
            _enemy.SetAttackTarget(other.gameObject.transform.position, unit);
        }
    }
}
