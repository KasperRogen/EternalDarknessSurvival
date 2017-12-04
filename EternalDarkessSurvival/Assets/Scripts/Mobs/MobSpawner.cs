using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MobSpawner : MonoBehaviour
{

    public int MaxMobs;
    public GameObject[] Mobs;
    public List<GameObject> CurrentMobs;
    public GameObject Player;
    public int spawnChance;

    public delegate void CheckDistanceToPlayer();

    public event CheckDistanceToPlayer OnSpawnMobs;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (Random.Range(0, 100) < spawnChance)
        {
            if (OnSpawnMobs != null)
                OnSpawnMobs.Invoke();


            if (CurrentMobs.Count < MaxMobs)
            {
                Vector3 spawnPos = RandomSpawnPos(200, 250);
                if (spawnPos != Vector3.zero)
                {
                    if(OnSpawnMobs == null) {

                        GameObject mob = Instantiate(Mobs[Random.Range(0, Mobs.Length)], spawnPos, Quaternion.identity);
                        mob.GetComponent<MobMovementScript>().MobSpawner = this;
                        mob.GetComponent<MobMovementScript>().Player = GameObject.FindGameObjectWithTag("Player");

                    } else if (OnSpawnMobs.GetInvocationList().Length < MaxMobs)
                    {
                        GameObject mob = Instantiate(Mobs[Random.Range(0, Mobs.Length)], spawnPos, Quaternion.identity);
                        mob.GetComponent<MobMovementScript>().MobSpawner = this;
                        mob.GetComponent<MobMovementScript>().Player = GameObject.FindGameObjectWithTag("Player");
                    }
                }
            }

        }
    }






    public Vector3 RandomSpawnPos(float minDistance, float maxDistance)
    {
        int index = 0;
        Vector3 randomDirection = Player.transform.position;

            randomDirection = UnityEngine.Random.insideUnitSphere * maxDistance;

            randomDirection += Player.transform.position;

            Debug.DrawLine(Player.transform.position, randomDirection, Color.red);

        if ((randomDirection - Player.transform.position).magnitude < maxDistance &&
            (randomDirection - Player.transform.position).magnitude > minDistance)
            return randomDirection;

        return Vector3.zero;
    }
}
