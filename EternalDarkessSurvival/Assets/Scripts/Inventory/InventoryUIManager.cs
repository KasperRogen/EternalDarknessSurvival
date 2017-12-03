using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour {

	public Inventory Inventory;
	public GameObject UIInventory;

	public GameObject InventoryFieldGameObject;

	List<ItemHolder> UIFieldItems;


	public InventoryUIManager(){
		UIFieldItems = new List<ItemHolder>();
	}

	void Start()
	{
		InitFieldItems();
		UpdateFieldItems();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.I)){
			UIInventory.SetActive(!UIInventory.active);
		}

		UpdateFieldItems();
	}

	// Initialize all FieldItems
	private void InitFieldItems(){
		// Go through all InventoryFieldGameObject children and find ItemHolders.
		for(int i = 0; i < InventoryFieldGameObject.transform.GetChildCount(); i++){
			UIFieldItems.Add(InventoryFieldGameObject.transform.GetChild(i).GetComponent<ItemHolder>());
		}
	}

	// Update All FieldItem ItemHolders
	void UpdateFieldItems(){
		// Retrieve all current field items
		RetrieveCurrentItemsInInventory();

		// Update the view.
		UIFieldItems.ForEach(i => i.UpdateItemUI());
	}

	// Retrieve all items from Inventory
	private void RetrieveCurrentItemsInInventory(){
		// Clear all ItemHolders in UIInvFields
		UIFieldItems.ForEach(i => i.ClearItemHolder());

		int index = 0;
		// Add FieldItems according to inventory items
		Inventory.Items.ForEach(i => UIFieldItems[index++].InitNewItem(i));
	}
	
	
}
