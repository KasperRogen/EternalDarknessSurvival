using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ClickMovement : MonoBehaviour
{

    private NavMeshAgent _agent;
	
	
	
	// Use this for initialization
	void Start ()
	{
		  _agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(1))
		{
            RaycastHit hit;
		    if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
		    _agent.SetDestination(hit.point);
		}
	}
}
