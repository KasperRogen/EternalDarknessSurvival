using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CostumInventoryDragger : MonoBehaviour, IPointerDownHandler, IPointerClickHandler {

	public Inventory Inventory;
	Image CurrentlyDraggedItem;
	Vector3 SavedLoc;
	bool Dragging = false;
	
	public RectTransform rectTransform;
	public RectTransform BottomRectTransform;

	
	
	void Update()
	{
		if(Dragging) CurrentlyDraggedItem.transform.position = Input.mousePosition;
		if(Input.GetMouseButtonUp(0) && Dragging) {
			Dragging = false;
			
			if(!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition) && CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item != null){
				if (RectTransformUtility.RectangleContainsScreenPoint(BottomRectTransform, Input.mousePosition) &&
				    CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item is ToolItem)
				{
					ItemHolder ih = CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>();

					Inventory.gameObject.GetComponent<ToolManager>().EquipTool(Inventory.gameObject.GetComponent<ToolManager>().ToolList.First(t => t.GatherType == (ih.Item as ToolItem).Tool.GatherType));
					
					CurrentlyDraggedItem.transform.position = SavedLoc;
					CurrentlyDraggedItem = null;
				
					return;
				}
				
				if (CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item is ToolItem)
				{
					Inventory.RemoveItem(CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item);
					Inventory.DropToolItem(CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item as ToolItem);
					if ((CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item as ToolItem).Tool.GatherType ==
					    Inventory.gameObject.GetComponent<ToolManager>().CharacterToolEquipped.GatherType)
					{
						ItemHolder ih = CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>();

						Inventory.gameObject.GetComponent<ToolManager>().EquipTool(Inventory.gameObject.GetComponent<ToolManager>().ToolList.First(t => t.GatherType == PublicEnums.ToolGatherType.None));
					}
				}
				else
				{
					Inventory.RemoveItem(CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item);
					Inventory.DropItem(CurrentlyDraggedItem.gameObject.GetComponent<ItemHolder>().Item);
				}
				
				
				CurrentlyDraggedItem.transform.position = SavedLoc;
				CurrentlyDraggedItem = null;
			}

			
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
	    Debug.Log(eventData.clickCount);
		Debug.Log(eventData.eligibleForClick);
		Debug.Log(eventData.pointerEnter);

	    if (eventData.pointerEnter.CompareTag("Draggable"))
	    {
		    OnBeginDrag(eventData.pointerEnter);
	    }
    }


	public void OnPointerClick(PointerEventData eventData)
	{	
		
		if (eventData.clickCount == 2)
		{
			Debug.Log("DoubleClicked!");
			if (eventData.pointerEnter.CompareTag("Draggable"))
			{
				Debug.Log("Item is draggable!");
				ItemHolder ih = eventData.pointerEnter.GetComponent<ItemHolder>();

				if (ih.Item is ToolItem)
					Debug.Log("Item is ToolItem!");
					Inventory.gameObject.GetComponent<ToolManager>().EquipTool((ih.Item as ToolItem).Tool);
			}
		}
	}
}
