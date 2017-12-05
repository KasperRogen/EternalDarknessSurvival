using UnityEngine;
using UnityEngine.AI;

public class ClickMovement : MonoBehaviour
{
    public Animator anim;
    private NavMeshAgent _agent;
    private Vector3 targetPos;
	
	
	// Use this for initialization
	void Start ()
	{
	    anim = this.GetComponent<Animator>();


	    foreach (Transform child in transform)
	    {
	        if (child.GetComponent<Animator>() != null)
	            anim = child.GetComponent<Animator>();
	    }

		  _agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(_agent.velocity.magnitude > 0.08f) { 
	        anim.SetFloat("speed", _agent.velocity.magnitude);
	        anim.speed = _agent.velocity.magnitude;
        }

        if ((transform.position - targetPos).magnitude < 1)
	        _agent.isStopped = true;
	    else
	        _agent.isStopped = false;

    }





    public void Move()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            targetPos = hit.point;
            targetPos.y = transform.position.y;
            _agent.SetDestination(targetPos);
        }
    }
}
