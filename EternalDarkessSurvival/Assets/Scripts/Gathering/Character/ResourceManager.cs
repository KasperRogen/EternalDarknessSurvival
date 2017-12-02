using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

	public int Rocks, Tree;
	
	// Use this for initialization
	void Start ()
	{
		Rocks = 0;
		Tree = 0;
	}

	public void AddResources(int resourceCount, PublicEnums.ResourceType ResourceType)
	{
		switch (ResourceType)
		{
			case PublicEnums.ResourceType.Stone:
				Rocks += resourceCount;
				break;
			
			case PublicEnums.ResourceType.Wood:
				Tree += resourceCount;
				break;
			
			
		}
	}
}
