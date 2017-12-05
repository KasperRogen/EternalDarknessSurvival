using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
	public List<Tool> ToolList;

	public Tool CharacterToolEquipped;

	private EntityStats PlayerStats;

	public Image EqImage;

	public GameObject rightHand;
	public GameObject equippedItem;

	void Start()
	{
		PlayerStats = GetComponent<EntityStats>();

		CharacterToolEquipped = ToolList.First(t => t.GatherType == PublicEnums.ToolGatherType.None);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			UpdatePlayerStats();
		}
	}

	public void EquipTool(Tool tool)
	{
		if (ToolList.Any(t => t.GatherType == tool.GatherType))
		{
			if (equippedItem != null)
			{
				Destroy(equippedItem.gameObject);
			}
			CharacterToolEquipped = ToolList.First(t => t.GatherType == tool.GatherType);
			GameObject instTool = Instantiate(PlayerStats.gameObject.GetComponent<Inventory>().ToolList
				.First(t => t.Tool.GatherType == tool.GatherType).DropPrefab);
			Instantiate(PlayerStats.gameObject.GetComponent<Inventory>().ToolList
				.First(t => t.Tool.GatherType == tool.GatherType).DropPrefab);
			
			instTool.transform.position = rightHand.transform.position;
			instTool.gameObject.GetComponent<ProximityPicker>().enabled = false;
			instTool.transform.SetParent(rightHand.transform);
			instTool.transform.rotation = rightHand.transform.rotation;

			equippedItem = instTool;
		}
		else
		{
			CharacterToolEquipped = ToolList.First(t => t.GatherType == PublicEnums.ToolGatherType.None);
			
			Destroy(equippedItem.gameObject);
			equippedItem = null;
		}
		UpdatePlayerStats();
	}

	public void UpdatePlayerStats()
	{
		PlayerStats.Damage = CharacterToolEquipped.Damage;
		PlayerStats.GatherDamage = CharacterToolEquipped.GatherDamage;

		if (CharacterToolEquipped.GatherType != PublicEnums.ToolGatherType.None)
		{
			EqImage.sprite = PlayerStats.gameObject.GetComponent<Inventory>().ToolList
				.First(t => t.Tool.GatherType == CharacterToolEquipped.GatherType).Sprite;
		}
		else
		{
			EqImage.sprite = null;
		}
	}

}
