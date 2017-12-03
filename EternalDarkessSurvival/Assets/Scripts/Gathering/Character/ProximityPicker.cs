using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityPicker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit[] sphereHit;
        sphereHit = Physics.SphereCastAll(transform.position, transform.localScale.x, Vector3.down);
        foreach (RaycastHit raycastHit in sphereHit)
        {
            if(raycastHit.transform.gameObject.tag == "Player"){
				gameObject.GetComponent<Gatherable>().Gather(gameObject.GetComponent<Gatherable>().resourceCount, raycastHit.transform.gameObject);
			}
        }
        
	}

	
}
