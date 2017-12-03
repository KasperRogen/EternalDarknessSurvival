using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuScript : MonoBehaviour {
    


    public GameObject ButtonTemplate;
    public List<CustomButton> buttons;
    public List<Button> Todisable;

	// Use this for initialization
	void Start () {

	    float YIndex = 1;

	    foreach (CustomButton customButton in buttons)
	    {
	        Vector3 positionvector = ButtonTemplate.GetComponent<RectTransform>().position;
	        positionvector.y -= ButtonTemplate.GetComponent<RectTransform>().rect.height / 3;

            GameObject button = Instantiate(ButtonTemplate,
	             positionvector, Quaternion.identity,
	            GameObject.Find("BuildingMenuCanvas").transform);

	        float _childYIndex = 0;
	        button.AddComponent<ButtonScript>().message = customButton;
	        button.GetComponent<ButtonScript>().BMS = this;
	        customButton.GO = button;

            foreach (CustomButton child in customButton.Children)
	        {
	            Vector3 childPosition = button.GetComponent<RectTransform>().position;
	            childPosition.x += (button.GetComponent<RectTransform>().rect.width) / 3;
	            childPosition.y -= (button.GetComponent<RectTransform>().rect.height * _childYIndex++) / 3;
	            customButton.childrenGOS.Add(InitializeButtons(childPosition, child, button.transform, child.Level));
            }
	    }




    }





    GameObject InitializeButtons(Vector3 position, CustomButton customButton, Transform parent, int parentLevel)
    {
        GameObject button = Instantiate(ButtonTemplate,
            position, Quaternion.identity,
            parent);
        button.transform.GetChild(0).GetComponent<Text>().text = customButton.Name;
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
                    childPosition.x += (button.GetComponent<RectTransform>().rect.width) / 3;
                    childPosition.y -= (button.GetComponent<RectTransform>().rect.height * _childYIndex++) / 3;
                    customButton.childrenGOS.Add(InitializeButtons(childPosition, child, button.transform, parentLevel + 1));
            }
        }

        return button;
    }

    public void UpdateMenu(CustomButton button)
    {

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
        
    }



    void CloseButtons(CustomButton buttonToAvoid, CustomButton toClose, int startFromLevel)
    {
        if (toClose.Children.Count > 0)
        {
            for (int i = 0; i < toClose.Children.Count; i++)
            {
                Debug.Log("Closing: " + toClose.Name + " Avoiding: " + buttonToAvoid.Name + " From level: " + startFromLevel + " | looking at: " + toClose.Children[i].Name + " With level: " + toClose.Children[i].Level);
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
}

