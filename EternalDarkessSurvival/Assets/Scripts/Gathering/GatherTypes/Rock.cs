using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Gatherable {

	// Use this for initialization
	void Start ()
	{
		resourceCount = 50;
		resourceType = PublicEnums.ResourceType.Stone;
	}

}
