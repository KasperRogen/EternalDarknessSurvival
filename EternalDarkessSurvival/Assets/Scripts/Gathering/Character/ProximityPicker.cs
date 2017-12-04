using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityPicker : MonoBehaviour
{
	private Inventory Inventory;
	
	// Update is called once per frame
	void Update () {
		RaycastHit[] sphereHit;
        sphereHit = Physics.SphereCastAll(transform.position, 0.3f, Vector3.down);
        foreach (RaycastHit raycastHit in sphereHit)
        {
            if(raycastHit.transform.gameObject.tag == "Player")
            {
	            Inventory = raycastHit.transform.gameObject.GetComponent<Inventory>();

	            if (Inventory.Items.Count < Inventory.Items.Capacity)
	            {
		            if (gameObject.GetComponent<ToolItem>() != null)
		            {
			            Debug.Log("Doing this!");
			            Inventory.AddToolItem(gameObject.GetComponent<ToolItem>());
			            
			            Destroy(gameObject);
			            return;
		            }
		            if (gameObject.GetComponent<Item>() != null)
		            {
			            Inventory.AddResourceItem(GetComponent<Item>().Quantity, GetComponent<Item>().ItemType);
			            Destroy(gameObject);
			            return;
		            }
	            }
            }
        }
        
	}

	
}
