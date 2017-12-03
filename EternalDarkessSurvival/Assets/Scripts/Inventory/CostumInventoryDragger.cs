using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CostumInventoryDragger : MonoBehaviour, IPointerDownHandler {

	public Inventory Inventory;
	Image CurrentlyDraggedItem;
	Vector3 SavedLoc;
	bool Dragging = false;
	
	public RectTransform rectTransform;

	void Start(){
	}

	void Update()
	{
		if(Dragging) CurrentlyDraggedItem.transform.position = Input.mousePosition;
		if(Input.GetMouseButtonUp(0) && Dragging) {
			Dragging = false;
			
			if(!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition) && CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item != null){			
				Inventory.RemoveItem(CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item);
				Inventory.DropItem(CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item);
			}

			CurrentlyDraggedItem.transform.position = SavedLoc;
			CurrentlyDraggedItem = null;
		}
	}

	// Use this for initialization
	public void OnBeginDrag(GameObject obj){
		if(obj.GetComponent<ItemHolder>().Item != null){
			CurrentlyDraggedItem = obj.GetComponent<Image>();
			SavedLoc = CurrentlyDraggedItem.transform.position;
			Dragging = true;
		}
	}

    public void OnPointerDown(PointerEventData eventData)
    {
		Debug.Log(eventData.eligibleForClick);
		Debug.Log(eventData.pointerEnter);

		if(eventData.pointerEnter.tag == "Draggable"){
			OnBeginDrag(eventData.pointerEnter);
		}
    }


}
