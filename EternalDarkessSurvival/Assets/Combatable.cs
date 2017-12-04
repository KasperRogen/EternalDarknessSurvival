using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatable : MonoBehaviour
{

    private float _lastAttack;
    public float AttackCooldown;

    private EntityStats stats;

    // Use this for initialization
    void Start()
    {
        _lastAttack = Time.time;
        stats = GetComponent<EntityStats>();
    }



    public void GetAttacked(GameObject Attacker, float damage)
    {
        stats.Health -= damage;
        if(stats.Health <= 0)
            Destroy(gameObject);
    }



    public void Attack(GameObject enemy)
    {
        if ((transform.position - enemy.transform.position).magnitude < 1.5f && Time.time - _lastAttack > AttackCooldown)
        {
            enemy.GetComponent<Combatable>().GetAttacked(gameObject, stats.Damage);
            _lastAttack = Time.time;
        }
    }
}
