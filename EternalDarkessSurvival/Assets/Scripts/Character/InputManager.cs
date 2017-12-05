using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private Combatable _combat;
    private EntityStats _stats;
    private BuildingManager _buildingManager;
    public Canvas BuildingMenuCanvas;
    private Transform target;
    private AudioSource _audioSource;

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

	            if (hit.transform.gameObject.GetComponent<Combatable>() != null && (transform.position - hit.transform.position).magnitude < GetComponent<EntityStats>().range && _stats.ReadyToSwing())
	            {
	                GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>().speed = 2;
	                GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>().SetTrigger("Swing");
	                StartCoroutine(RotateTo(hit.transform.gameObject, hit.point));
	            }
	            else if (hit.transform.gameObject.GetComponent<Gatherable>() != null && (transform.position - hit.transform.position).magnitude < GetComponent<EntityStats>().range && _stats.ReadyToSwing())
	            {
	                GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>().speed = 2;
	                GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>().SetTrigger("Swing");
                    StartCoroutine(RotateTo(hit.transform.gameObject, hit.point));
	            }
	        }


	    } else if (Input.GetMouseButtonDown(1))
	    {
	        if (_buildingManager.IsBuilding)
	        {
	            _buildingManager.StopBuild();
	            return;
	        }
	        else
	        {
	            GetComponent<ClickMovement>().Move();
	        }
        }


	    if (Input.GetMouseButton(1))
	    {
	        GetComponent<ClickMovement>().Move();
        }
        
	}



    IEnumerator RotateTo(GameObject target, Vector3 point) {
        for(int i = 0; i < 20; i++) {
            GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>().speed = 3;
            var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }


        if(target.GetComponent<Gatherable>() != null) { 

        GetComponent<CharacterGatherController>().GatherResource();

            if(target.GetComponent<Gatherable>() != null) { 
                Gatherable gatherObject = target.GetComponent<Gatherable>();

                if (gatherObject != null)
                {
                    // Gatherable object found and clicked on! Do shit.
                    if (gatherObject.resourceType == PublicEnums.ItemType.Stone)
                        Instantiate(GetComponent<CharacterGatherController>().StoneParticleSystem, point, Quaternion.identity);
                    else if (gatherObject.resourceType == PublicEnums.ItemType.Wood)
                        Instantiate(GetComponent<CharacterGatherController>().WoodParticleSystem, point, Quaternion.identity);
                    if(gatherObject.GetComponent<AudioSource>() != null)
                        gatherObject.GetComponent<AudioSource>().Play();
                }
            }

            target.GetComponent<Gatherable>().Gather(gameObject);

        } else if (target.GetComponent<Combatable>() != null)
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            _combat.Attack(target.transform.gameObject);
            Instantiate(GetComponent<Combatable>().BloodParticles, point, Quaternion.identity);
            if (target.GetComponent<AudioSource>() != null)
                target.GetComponent<AudioSource>().Play();
        }

    }

    

}
