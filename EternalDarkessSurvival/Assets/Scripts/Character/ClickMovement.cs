using UnityEngine;
using UnityEngine.AI;

public class ClickMovement : MonoBehaviour
{

    private NavMeshAgent _agent;
    private Vector3 targetPos;
	
	
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
		    if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
		        targetPos = hit.point;
		        targetPos.y = transform.position.y;
                _agent.SetDestination(targetPos);
		    }
            
		}

	    if ((transform.position - targetPos).magnitude < 1)
	        _agent.isStopped = true;
	    else
	        _agent.isStopped = false;

    }
}
