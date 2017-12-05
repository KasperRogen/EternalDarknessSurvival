using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuScript : MonoBehaviour {
    


    public GameObject ButtonTemplate;
    public List<CustomButton> buttons;
    public List<Button> Todisable;
    public Inventory Inventory;

	// Use this for initialization
	void Start () {

	    float YIndex = 1;

	    Vector3 positionvector = ButtonTemplate.GetComponent<RectTransform>().position;
        foreach (CustomButton customButton in buttons)
	    {

	        positionvector.y -= ButtonTemplate.GetComponent<RectTransform>().rect.height;


	        GameObject button = Instantiate(ButtonTemplate,
	            GameObject.Find("BuildingMenuCanvas").transform.GetChild(0).transform, false);

	        button.transform.GetChild(0).GetComponent<Text>().text = customButton.Name;
	        button.transform.position = positionvector;

            float _childYIndex = 0;
	        button.AddComponent<ButtonScript>().message = customButton;
	        button.GetComponent<ButtonScript>().BMS = this;
	        customButton.GO = button;

            foreach (CustomButton child in customButton.Children)
	        {
	            Vector3 childPosition = button.GetComponent<RectTransform>().position;
	            childPosition.x += (button.GetComponent<RectTransform>().rect.width);
	            childPosition.y -= (button.GetComponent<RectTransform>().rect.height * _childYIndex++);
	            customButton.childrenGOS.Add(InitializeButtons(childPosition, child, button.transform, child.Level));
            }
	    }




    }





    GameObject InitializeButtons(Vector3 position, CustomButton customButton, Transform parent, int parentLevel)
    {
        GameObject button = Instantiate(ButtonTemplate,
            position, Quaternion.identity,
            parent);

        Text txt = button.transform.GetChild(0).GetComponent<Text>();

        txt.text = customButton.Name;
        

        if (customButton.ToPlace != null && customButton.ToPlace.GetComponent<DeployableStats>() != null)
        {
            Debug.Log(customButton.Name);
            if (customButton.ToPlace.GetComponent<DeployableStats>().WoodPrice != 0)
            {
                txt.text += "\nWood: " + customButton.ToPlace.GetComponent<DeployableStats>().WoodPrice;
            }

            if (customButton.ToPlace.GetComponent<DeployableStats>().StonePrice != 0)
            {
                txt.text += "\nStone: " + customButton.ToPlace.GetComponent<DeployableStats>().StonePrice;
            }
        }



        button.name = customButton.Name;
        customButton.Parent = parent;
        customButton.Level = parentLevel + 1;

        button.AddComponent<ButtonScript>().message = customButton;
        button.GetComponent<ButtonScript>().BMS = this;
        customButton.GO = button;
        //Button.SetActive(false);

        customButton.GO.gameObject.SetActive(false);

        if (customButton.Children.Any())
        {
                float _childYIndex = 0;
                foreach (CustomButton child in customButton.Children)
                {
                    Vector3 childPosition = button.GetComponent<RectTransform>().position;
                    childPosition.x += (button.GetComponent<RectTransform>().rect.width);
                    childPosition.y -= (button.GetComponent<RectTransform>().rect.height * _childYIndex++);
                    customButton.childrenGOS.Add(InitializeButtons(childPosition, child, button.transform, parentLevel + 1));
            }
        }

        return button;
    }

    public void UpdateMenu(CustomButton button)
    {
        int currentWood = 0;
        int currentStone = 0;

        if ( button.ToCraft != null && button.ToCraft.GetComponent<ToolItem>() != null)
        {
            if (Inventory.Items.Any(i => (i).ItemType == PublicEnums.ItemType.Wood))
            {
                currentWood = Inventory.Items.Where(i => i.ItemType == PublicEnums.ItemType.Wood).Sum(i => i.Quantity);
            }
            if (Inventory.Items.Any(i => i.ItemType == PublicEnums.ItemType.Stone))
            {
                currentStone = Inventory.Items.Where(i => i.ItemType == PublicEnums.ItemType.Stone)
                    .Sum(i => i.Quantity);
            }
        }

        Debug.Log("THIS NOW PRESSED: " + button.Name);

        foreach (CustomButton customButton in buttons)
        {
            CloseButtons(button, customButton, button.Level);
        }

        if (button.ToPlace != null)
        {
            GetComponent<BuildingManager>().IsBuilding = true;
            GetComponent<BuildingManager>().BuildingObject = button.ToPlace;
        }

        if (button.ToCraft != null && button.Name == "Pickaxe" || button.Name == "Axe" || button.Name == "Sword")
        {
            if (button.ToCraft.GetComponent<DeployableStats>().WoodPrice <= currentWood && button.ToCraft.GetComponent<DeployableStats>().StonePrice <= currentStone)
            {
                Inventory.DecrementResource(PublicEnums.ItemType.Wood, button.ToCraft.GetComponent<DeployableStats>().WoodPrice);
                Inventory.DecrementResource(PublicEnums.ItemType.Stone, button.ToCraft.GetComponent<DeployableStats>().StonePrice);

                Inventory.AddToolItem(button.ToCraft.GetComponent<ToolItem>());
            }
        }
        


    }



    void CloseButtons(CustomButton buttonToAvoid, CustomButton toClose, int startFromLevel)
    {
        if (toClose.Children.Count > 0)
        {
            for (int i = 0; i < toClose.Children.Count; i++)
            {
                //Debug.Log("Closing: " + toClose.Name + " Avoiding: " + buttonToAvoid.Name + " From level: " + startFromLevel + " | looking at: " + toClose.Children[i].Name + " With level: " + toClose.Children[i].Level);
                if (toClose.Children[i].Name != buttonToAvoid.Name && toClose.Children[i].Level > startFromLevel &&
                    toClose.Children[i].Parent != buttonToAvoid.GO.transform)
                {
                    toClose.childrenGOS[i].gameObject.SetActive(false);
                    toClose.Children[i].IsDrawn = false;
                }
                else if(toClose.Children[i].Parent == buttonToAvoid.GO.transform)
                {
                    
                    toClose.childrenGOS[i].SetActive(toClose.Children[i].IsDrawn ^= true);
                }
                CloseButtons(buttonToAvoid, toClose.Children[i], startFromLevel);
            }
        }
    }

}




    



[System.Serializable]
public class CustomButton
{
    public string Name;
    public Transform Parent;
    public int Level;
    public GameObject GO;
    public Sprite image;
    public bool IsDrawn;
    public List<GameObject> childrenGOS;
    public List<CustomButton> Children;
    public GameObject ToPlace;
    public GameObject ToCraft;
}

