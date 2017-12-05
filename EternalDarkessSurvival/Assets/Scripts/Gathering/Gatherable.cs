using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
	public float resourceCount = 50;
	public PublicEnums.ItemType resourceType;
	public PublicEnums.TerrainType TerrainType;

	public void Gather(GameObject player)
	{

		int resourceGained = player.GetComponent<EntityStats>().GatherDamage;

		if(resourceCount - resourceGained < 0){
			resourceGained = (int)resourceCount;
		}
		
		resourceCount -= resourceGained;

		player.transform.GetComponent<Inventory>().AddResourceItem(resourceGained, resourceType);
		
		if (resourceCount <= 0)
		{
			Destroy(gameObject);
		}
	}
}
