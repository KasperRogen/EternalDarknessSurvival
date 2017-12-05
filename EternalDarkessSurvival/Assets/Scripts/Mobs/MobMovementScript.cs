using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MobMovementScript : MonoBehaviour
{
    public float seeDistance;
    private Vector3 _wanderPosition;
    private Combatable _combat;
    private EntityStats _stats;
    public MobSpawner MobSpawner;
    public GameObject Player;

    private NavMeshAgent _navMeshAgent;

	// Use this for initialization
	void Start ()
	{
	    _stats = GetComponent<EntityStats>();
	    _combat = GetComponent<Combatable>();
	    _navMeshAgent = GetComponent<NavMeshAgent>();

	    MobSpawner.OnSpawnMobs += DestroyByDistance;

	}



    public void DestroyByDistance()
    {
        if ((transform.position - Player.transform.position).magnitude > 300)
        {
            DestroyThis();
        }
    }




    public void DestroyThis()
    {
        MobSpawner.OnSpawnMobs -= DestroyThis;
        Destroy(gameObject);
    }




	
	// Update is called once per frame
	void Update ()
	{


	    GetComponent<Animator>().speed = _navMeshAgent.velocity.magnitude / 3;


        List<GameObject> VisibleLights;
	    GameObject player;

	    if ((player = FindPlayer()) != null)
	    {
	        if ((player.transform.position - transform.position).magnitude > _stats.range - 0.5f)
	        {
	            _navMeshAgent.SetDestination(player.transform.position);
	        }
            else { 
                _navMeshAgent.SetDestination(transform.position);
                _combat.Attack(player);
	        }



        } else if ((VisibleLights = FindLights()).Any())
	    {
	        if ((FindClosestLight(VisibleLights).transform.position - transform.position).magnitude > 0.5f)
	        {
	            _navMeshAgent.SetDestination(FindClosestLight(VisibleLights).transform.position);
	        }
	        else
	        {
	            _navMeshAgent.SetDestination(transform.position);
	        }
	    }




	    else if(_wanderPosition == Vector3.zero || (transform.position - _wanderPosition).magnitude < 2)
	    {
	        _wanderPosition = RandomNavSphere(transform.position, seeDistance, 0);
            _navMeshAgent.SetDestination(_wanderPosition);
	    }


	}


    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        int counter = 0;
        NavMeshHit navHit;

        do
        {

            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

            randomDirection += origin;

            NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);
            counter++;
            if(Single.IsInfinity(navHit.position.x))
            Debug.Log(navHit.position);
        } while (Single.IsInfinity(navHit.position.x) || counter < 100);


        return navHit.position;
    }



    GameObject FindPlayer()
    {
        GameObject player = null;
        RaycastHit[] hit;
        if ((hit = Physics.SphereCastAll(transform.position, seeDistance, Vector3.down)) != null)
        {

            if (hit.Any(x => x.transform.gameObject.tag == "Player")) { 
                player = hit.First(x => x.transform.gameObject.tag == "Player").transform.gameObject;

            RaycastHit firstSeenObject;
            Physics.Raycast(transform.position, player.transform.position - transform.position, out firstSeenObject);
            if (firstSeenObject.transform.gameObject != player)
            {
                return null;
            }
                Debug.DrawLine(transform.position, player.transform.position, Color.green);
        }
        return player;
        }
        return null;
    }



    GameObject FindClosestLight(List<GameObject> lights)
    {
        int bestLight = 0;
        float closestDistance = (transform.position - lights[0].transform.position).magnitude;

        for (int i = 0; i < lights.Count; i++)
        {
            if ((lights[i].transform.position - transform.position).magnitude < closestDistance)
            {
                bestLight = i;
                closestDistance = (lights[i].transform.position - transform.position).magnitude;
            }
        }

        float bestVal = lights.Min(y => (y.transform.position - transform.position).magnitude);
        return lights.First(x => Math.Abs((x.transform.position - transform.position).magnitude - bestVal) < 0.1);
    }


    List<GameObject> FindLights()
    {
        List<RaycastHit> lights = new List<RaycastHit>();
        RaycastHit[] hit;
        if ((hit = Physics.SphereCastAll(transform.position, seeDistance, Vector3.down)) != null)
        {
            lights = hit.Where(x => x.transform.gameObject.tag == "Light").ToList();
            for (int i = 0; i < lights.Count; i++)
            {
                RaycastHit firstSeenObject;
                Physics.Raycast(transform.position, lights[i].transform.position - transform.position,
                    out firstSeenObject);
                if (firstSeenObject.transform.gameObject != lights[i].transform.gameObject)
                {
                    lights.RemoveAt(i);
                    i--;
                }
                else
                {
                    Debug.DrawLine(transform.position, lights[i].transform.position);
                }
            }
        }

        List<GameObject> visibleLights = new List<GameObject>();
        foreach (RaycastHit raycastHit in lights)
        {
            visibleLights.Add(raycastHit.transform.gameObject);
        }

        return visibleLights;
    }


}
