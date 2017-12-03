using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
	public float resourceCount = 50;
	public PublicEnums.ItemType resourceType;
	
	// Use this for initialization
	void Start ()
	{

	}

	public void Gather(float damage, GameObject player)
	{
		int resourceGained = (int) damage;
		resourceCount -= damage;

		player.transform.GetComponent<Inventory>().AddResourceItem(resourceGained, resourceType);
		
		if (resourceCount <= 0)
		{
			Destroy(gameObject);
		}
	}
}
