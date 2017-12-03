using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MobMovementScript : MonoBehaviour
{
    public float seeDistance;
    private Vector3 _wanderPosition;

    private NavMeshAgent _navMeshAgent;

	// Use this for initialization
	void Start ()
	{
	    _navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        List<GameObject> VisibleLights;
	    GameObject player;

	    if ((player = FindPlayer()) != null)
	    {
	        if ((player.transform.position - transform.position).magnitude > player.GetComponent<Collider>().bounds.size.x/2 + transform.gameObject.GetComponent<Collider>().bounds.size.x)
	        {
	            _navMeshAgent.SetDestination(player.transform.position);
	            transform.LookAt(player.transform.position);
	        }
	        else
                _navMeshAgent.SetDestination(transform.position);



        } else if ((VisibleLights = FindLights()).Any())
	    {
	        if ((FindClosestLight(VisibleLights).transform.position - transform.position).magnitude > 0.5f)
	        {
	            _navMeshAgent.SetDestination(FindClosestLight(VisibleLights).transform.position);
	            transform.LookAt(FindClosestLight(VisibleLights).transform.position);
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
