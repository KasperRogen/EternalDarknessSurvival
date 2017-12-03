using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterGatherController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			GatherResource();
		}
	}

	void GatherResource()
	{
		RaycastHit rayHit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out rayHit))
		{
			float distanceToResource = (rayHit.transform.position - transform.position).magnitude;
			//Debug.Log(distanceToResource);
			if (distanceToResource < 1.7)
			{
				Debug.DrawLine(transform.position, rayHit.transform.position);
				Debug.Log(rayHit.GetType());
				Gatherable gatherObject = rayHit.transform.GetComponent<Gatherable>();
				if (gatherObject != null)
				{
					// Gatherable object found and clicked on! Do shit.
					gatherObject.Gather(10, gameObject);
				}
			}
		}
	}
}
