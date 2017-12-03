using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemHolder : MonoBehaviour {
	public Item Item;
	private Image Renderer;
	private Text TextRenderer;
	// Use this for initialization

	void Start () {
		Renderer = gameObject.GetComponent<Image>();
		TextRenderer = gameObject.GetComponentInChildren<Text>();
	}
	
	public void ClearItemHolder(){
		Item = null;
		Renderer.sprite = null;
		TextRenderer.text = "";
	}

	public void InitNewItem(Item item){
		ClearItemHolder();
		Item = item;
	}

	public void UpdateItemUI(){
		if(Item != null){
			Renderer.sprite = Item.Sprite;
			TextRenderer.text = Item.Quantity.ToString();
		}
	}
}
