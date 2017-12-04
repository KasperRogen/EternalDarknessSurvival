using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private Combatable _combat;
    private EntityStats _stats;
    private BuildingManager _buildingManager;
    public Canvas BuildingMenuCanvas;


	// Use this for initialization
	void Start ()
	{
	    _combat = GetComponent<Combatable>();
	    _stats = GetComponent<EntityStats>();
	    _buildingManager = BuildingMenuCanvas.GetComponent<BuildingManager>();
	}
	
	// Update is called once per frame
	void Update () {


	    if (Input.GetMouseButtonDown(0))
	    {
	        RaycastHit hit;

	        if (_buildingManager.IsBuilding)
	        {
                _buildingManager.AttemptBuild();
	            return;
	        }

            

	        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
	        {
                Debug.Log(hit.transform.name);

	            if (hit.transform.gameObject.GetComponent<Combatable>() != null && (transform.position - hit.transform.position).magnitude < 1.5f)
	            {
	                _combat.Attack(hit.transform.gameObject);
	            }
	            else if (hit.transform.gameObject.GetComponent<Gatherable>() != null)
	            {
	                hit.transform.gameObject.GetComponent<Gatherable>().Gather(_stats.Damage, gameObject);
	            }
	        }


	    } else if (Input.GetMouseButtonDown(1))
	    {
	        if (_buildingManager.IsBuilding)
	        {
	            _buildingManager.StopBuild();
	            return;
	        }
        }

	}

}
