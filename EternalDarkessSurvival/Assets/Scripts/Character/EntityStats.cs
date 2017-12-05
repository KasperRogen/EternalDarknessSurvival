using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EntityStats : MonoBehaviour
{

    public float Health = 100;
    public float Damage = 5;
    public int GatherDamage = 3;
    public float range = 2;
    private float _lastattack;

    void Start()
    {
        _lastattack = Time.time;
    }


    public bool ReadyToSwing()
    {
        if ((Time.time - _lastattack) > GetComponent<Combatable>().AttackCooldown)
        {
            _lastattack = Time.time;
            return true;
        }
        return false;
    }



}
