using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatable : MonoBehaviour
{
    public ParticleSystem BloodParticles;
    private float _lastAttack;
    public float AttackCooldown;

    private EntityStats stats;
    public GameObject c1D;
    public GameObject c2D;
    public GameObject d1E;
    
    // Use this for initialization
    void Start()
    {
        _lastAttack = Time.time;
        stats = GetComponent<EntityStats>();
    }

    private void DIE()
    {
        if (c1D != null)
        {
            c1D.SetActive(false);
        }
        
        c2D.SetActive(false);
        d1E.SetActive(true);
    }

    public void GetAttacked(GameObject Attacker, float damage)
    {
        stats.Health -= damage;
        Instantiate(BloodParticles, transform.position, Quaternion.identity);
        if (stats.Health <= 0)
        {
            Destroy(gameObject);
            DIE();
        }

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
