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
			CharacterToolEquipped = ToolList.First(t => t.GatherType == tool.GatherType);
		}
		else
		{
			CharacterToolEquipped = ToolList.First(t => t.GatherType == PublicEnums.ToolGatherType.None);
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
