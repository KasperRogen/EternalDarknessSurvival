using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatable : MonoBehaviour
{
    public ParticleSystem BloodParticles;
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
        Instantiate(BloodParticles, transform.position, Quaternion.identity);
        if(stats.Health <= 0) {
            if(GetComponent<MobMovementScript>() != null)
            {
                GetComponent<MobMovementScript>().DestroyThis();
                return;
            }

            Destroy(gameObject);
        }
    }



    public void Attack(GameObject enemy)
    {
        if ((transform.position - enemy.transform.position).magnitude < stats.range && Time.time - _lastAttack > AttackCooldown)
        {
            enemy.GetComponent<Combatable>().GetAttacked(gameObject, stats.Damage);
            _lastAttack = Time.time;
        }
    }
}
