using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
	public float resourceCount = 50;
	public PublicEnums.ItemType resourceType;
	public PublicEnums.TerrainType TerrainType;

	public void Gather(float damage, GameObject player)
	{
		int resourceGained = (int) damage;

		if(resourceCount - damage < 0){
			resourceGained = (int)resourceCount;
		}
		
		resourceCount -= damage;

		player.transform.GetComponent<Inventory>().AddResourceItem(resourceGained, resourceType);
		
		if (resourceCount <= 0)
		{
			Destroy(gameObject);
		}
	}
}
