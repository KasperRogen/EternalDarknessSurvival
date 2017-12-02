using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
	public float resourceCount;
	public PublicEnums.ResourceType resourceType;
	
	// Use this for initialization
	void Start ()
	{
		resourceCount = 30;
	}

	public void Gather(float damage, GameObject player)
	{
		int resourceGained = (int) damage;
		resourceCount -= damage;

		player.transform.GetComponent<ResourceManager>().AddResources(resourceGained, resourceType);
		
		if (resourceCount <= 0)
		{
			Destroy(gameObject);
		}
	}
}
