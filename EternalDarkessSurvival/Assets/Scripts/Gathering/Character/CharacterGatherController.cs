using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework.Constraints;
using UnityEngine;

public class CharacterGatherController : MonoBehaviour
{
    public ParticleSystem StoneParticleSystem;
    public ParticleSystem WoodParticleSystem;

	private EntityStats EntityStats;
	private ToolManager ToolManager;
	public GenerateTerrainFromNoise TerrainManager;
	
    public float MiningDistance = 2.5f;

	void Start()
	{
		EntityStats = GetComponent<EntityStats>();
		ToolManager = GetComponent<ToolManager>();
		
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetMouseButtonDown(0))
		//{
		//	GatherResource();
		
	}

	public void GatherResource()
	{
		RaycastHit rayHit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out rayHit))
		{
			float distanceToResource = (rayHit.transform.position - transform.position).magnitude;
			//Debug.Log(distanceToResource);
			if (distanceToResource < MiningDistance)
			{
				Debug.DrawLine(transform.position, rayHit.transform.position);
				//Debug.Log(rayHit.GetType());
				Gatherable gatherObject = rayHit.transform.GetComponent<Gatherable>();
			}
		}
	}
}
