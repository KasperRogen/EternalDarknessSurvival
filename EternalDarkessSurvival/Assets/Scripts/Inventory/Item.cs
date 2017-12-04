using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Item : MonoBehaviour {
	public PublicEnums.ItemType ItemType;
	public GameObject DropPrefab;
    public Sprite Sprite;
    public string Name;
    public int Quantity = 0;
    public int MaxQuantity = 20;

}
