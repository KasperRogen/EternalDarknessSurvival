using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour {
	public List<Item> Items;
	public List<Item> ItemsList;

	public GameObject DropPrefab;

	public Inventory(){
		Items = new List<Item>(15);
	}

	// Add new items to inventory
	public void AddNewItem(Item item){
		if(Items.Count < Items.Capacity){
			Items.Add(item);
		}else{
			DropItem(item);
		}
	}


    public void DecrementResource(PublicEnums.ItemType type, int quantity)
    {
		Item ItemToPick = Items.Last(i => i.ItemType == type);
		if(ItemToPick.Quantity - quantity >= 0){
			ItemToPick.Quantity -= quantity;
			if(ItemToPick.Quantity <= 0){
				RemoveItem(ItemToPick);
			}
			return;
		}
		if(ItemToPick.Quantity - quantity < 0){
			ItemToPick.Quantity -= ItemToPick.Quantity;
			int newquantity = (quantity - ItemToPick.Quantity) * (-1);
			DecrementResource(type, newquantity);
		}
    }



	// Remove items from inventory
	public void RemoveItem(Item removeItem){
		if(Items.Any() && removeItem != null){
		foreach(Item item in Items){
			if(removeItem.ItemType == item.ItemType && removeItem.Quantity == item.Quantity){
				Items.Remove(item);
							return;

			}
		}
		}
			
	}

	// Drop Item if inv is full
	public void DropItem(Item item){
		DropPrefab.GetComponent<Gatherable>().resourceCount = item.Quantity;
		DropPrefab.GetComponent<Gatherable>().resourceType = item.ItemType;
		Instantiate(DropPrefab, transform.position + Vector3.forward, transform.rotation);
	}

	// Add ResourceItems
	public void AddResourceItem(int quantity, PublicEnums.ItemType resourceType){
		// See if any items (That isn't full) of same type already exists => Add to this and return
		foreach(Item item in Items){
			if(item.ItemType == resourceType && item.Quantity < item.MaxQuantity){
				// Check the item quantity after adding
				int quantityAfterInc = item.Quantity + quantity;

				// if the quantity is more => use remainder method
				if(quantityAfterInc > item.MaxQuantity){
					int quantityRemainder = item.MaxQuantity - item.Quantity;
					int quantityNewAdd = (quantityRemainder - quantity)  * (-1);

					item.Quantity = item.MaxQuantity;
					
					AddResourceItem(quantityNewAdd, resourceType);
					return;
				}

				// If quantity is lower => just add
				if(quantityAfterInc < item.MaxQuantity){
					item.Quantity += quantity;
					return;
				}
			}
		}

		// Else => Create a new resource of this type
		Item newResourceItem = Instantiate(ItemsList.First(i => i.ItemType == resourceType));
		newResourceItem.Quantity = quantity;
		AddNewItem(newResourceItem);
	}


	
}
